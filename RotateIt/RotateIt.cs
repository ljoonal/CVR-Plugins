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

		private static Quaternion GrabbedRotation = Quaternion.identity;

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

		private Vector3 GetRotationInput()
		{
			Vector3 inputs = new();

			if (Input.GetKey(KeyPitchRight.Value)) inputs.x += RotationSpeed.Value;
			if (Input.GetKey(KeyPitchLeft.Value)) inputs.x -= RotationSpeed.Value;

			if (Input.GetKey(KeyYawRight.Value)) inputs.y += RotationSpeed.Value;
			if (Input.GetKey(KeyYawLeft.Value)) inputs.y -= RotationSpeed.Value;

			if (Input.GetKey(KeyRollRight.Value)) inputs.z += RotationSpeed.Value;
			if (Input.GetKey(KeyRollLeft.Value)) inputs.z -= RotationSpeed.Value;

			return inputs;
		}

		[HarmonyPatch(typeof(CVRPickupObject), "Update")]
		[HarmonyPostfix]
		public static void GrabbedObjectPatch(ref CVRPickupObject __instance)
		{
			// Need to only run when the object is grabbed by the local player
			if (__instance._controllerRay is null) return;

			Vector3 rotationInputs = Instance.GetRotationInput();

			if (rotationInputs != Vector3.zero)
			{
				Transform referenceTransform = __instance._controllerRay.transform;
				Quaternion rotationInputQuaternion = Quaternion.identity;
				rotationInputQuaternion *= Quaternion.AngleAxis(rotationInputs.x, referenceTransform.up);
				rotationInputQuaternion *= Quaternion.AngleAxis(rotationInputs.y, referenceTransform.right);
				rotationInputQuaternion *= Quaternion.AngleAxis(rotationInputs.z, referenceTransform.forward);

				GrabbedRotation *= rotationInputQuaternion;
#if DEBUG
				Instance.Logger.LogInfo(
					$"Adding rotations {rotationInputs} = {rotationInputQuaternion.eulerAngles}, result = {GrabbedRotation.eulerAngles}");
#endif
			}

			__instance.transform.rotation *= GrabbedRotation;
		}

		[HarmonyPatch(typeof(CVRPickupObject), "Grab")]
		[HarmonyPostfix]
		public static void OnGrabObject()
		{
			GrabbedRotation = Quaternion.identity;
		}
	}
}
