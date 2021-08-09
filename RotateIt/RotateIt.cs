using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;
using CVRPickupObject = ABI.CCK.Components.CVRPickupObject;

namespace RotateIt
{

	[BepInPlugin(BuildInfo.GUID, BuildInfo.Name, BuildInfo.Version)]
	[BepInProcess("ChilloutVR.exe")]
	public class RotateItPlugin : BaseUnityPlugin
	{
		private static RotateItPlugin Instance;
		private readonly ConfigEntry<KeyCode> KeyPitchRight, KeyPitchLeft, KeyYawRight, KeyYawLeft, KeyRollRight, KeyRollLeft;
		private readonly ConfigEntry<float> RotationSpeed;

		private static Quaternion GrabbedRotationOffset = Quaternion.identity;

		private ConfigEntry<KeyCode> RegisterKeybind(string prefName, KeyCode keyCode, string description)
		{
			const string keybindCategory = "Keybinds";
			return Config.Bind(keybindCategory, prefName, keyCode, "The key for: " + description);
		}

		RotateItPlugin()
		{
			KeyPitchLeft = RegisterKeybind("PitchLeft", KeyCode.I, "rotating the pitch of grabbed things left");
			KeyPitchRight = RegisterKeybind("PitchRight", KeyCode.K, "rotating the pitch of grabbed things right");
			KeyYawLeft = RegisterKeybind("YawLeft", KeyCode.J, "rotating the yaw of grabbed things left");
			KeyYawRight = RegisterKeybind("YawRight", KeyCode.L, "rotating the yaw of grabbed things right");
			KeyRollLeft = RegisterKeybind("RollLeft", KeyCode.U, "rotating the roll of grabbed things left");
			KeyRollRight = RegisterKeybind("RollRight", KeyCode.O, "rotating the roll of grabbed things right");

			RotationSpeed = Config.Bind("Settings", "RotationSpeed", 2f, "The speed with which to rotate objects");

			Instance = this;
		}

		public void Awake()
		{
			try
			{
				Harmony.CreateAndPatchAll(typeof(RotateItPlugin));
			}
			catch (System.Exception ex)
			{
				Logger.LogError($"Failed to apply patch: {ex}");
			}
		}

		private (float, float, float) GetRotationInput()
		{

			float pitch = 0f;
			if (Input.GetKey(KeyPitchRight.Value)) pitch += RotationSpeed.Value;
			if (Input.GetKey(KeyPitchLeft.Value)) pitch -= RotationSpeed.Value;

			float yaw = 0f;
			if (Input.GetKey(KeyYawRight.Value)) yaw += RotationSpeed.Value;
			if (Input.GetKey(KeyYawLeft.Value)) yaw -= RotationSpeed.Value;

			float roll = 0f;
			if (Input.GetKey(KeyRollRight.Value)) roll += RotationSpeed.Value;
			if (Input.GetKey(KeyRollLeft.Value)) roll -= RotationSpeed.Value;

			return (pitch, yaw, roll);
		}

		[HarmonyPatch(typeof(CVRPickupObject), "Update")]
		[HarmonyPostfix]
		public static void GrabbedObjectPatch(ref CVRPickupObject __instance)
		{
			// Need to only run when the object is grabbed by the local player
			if (__instance._controllerRay is null) return;

			__instance.transform.rotation *= GrabbedRotationOffset;

			Quaternion originalRotation = __instance.transform.rotation;
			Transform referenceTransform = ABI_RC.Core.Player.PlayerSetup.Instance.desktopCamera.transform;

			(float pitch, float yaw, float roll) = Instance.GetRotationInput();
			__instance.transform.RotateAround(__instance.transform.position, referenceTransform.right, pitch);
			__instance.transform.RotateAround(__instance.transform.position, referenceTransform.up, yaw);
			__instance.transform.RotateAround(__instance.transform.position, referenceTransform.forward, roll);

			// Add the new difference between the og rotation and our newly added rotation the the stored offset.
			GrabbedRotationOffset *= Quaternion.Inverse(__instance.transform.rotation) * originalRotation;
		}

		[HarmonyPatch(typeof(CVRPickupObject), "Grab")]
		[HarmonyPostfix]
		public static void OnGrabObject()
		//public static void OnGrabObject(CVRPickupObject __instance)
		{
			//GrabbedRotationOffset = __instance.transform.rotation;
			GrabbedRotationOffset = Quaternion.identity;
		}
	}
}
