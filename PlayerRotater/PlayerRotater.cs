using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;

namespace PlayerRotater
{
	internal enum EnabledState
	{
		Off,
		Toggled,
		Holding
	}

	[BepInPlugin(BuildInfo.GUID, BuildInfo.Name, BuildInfo.Version)]
	[BepInProcess("ChilloutVR.exe")]
	public class PlayerRotaterPlugin : BaseUnityPlugin
	{
		private static PlayerRotaterPlugin Instance;
		private ConfigEntry<KeyboardShortcut> mouseModeToggleKeybind;
		private ConfigEntry<KeyCode> mouseModeHoldKey;
		private Vector3? originalRotation = null;
		private Transform PlayerAvatarTransform
		{
			get
			{
				return ABI_RC.Core.Player.PlayerSetup.Instance._movementSystem.transform;
			}
		}

		private EnabledState _mouseLookEnabledField = EnabledState.Off;

		private EnabledState MouseLookEnabled
		{
			get
			{
				return _mouseLookEnabledField;
			}
			set
			{
				if (_mouseLookEnabledField != value)
					Logger.LogInfo($"Mouse look set to {value}");

				_mouseLookEnabledField = value;
			}
		}

		public void Awake()
		{
			const string inputPrefsCategory = "Inputs";

			mouseModeToggleKeybind = Config.Bind(
				inputPrefsCategory,
				"KeybindMouseMode",
				new KeyboardShortcut(KeyCode.None),
				"The keybind for toggling mouse mode");

			mouseModeHoldKey = Config.Bind(
				inputPrefsCategory,
				"KeycodeMouseModeHold",
				KeyCode.Mouse2,
				"A key that enables mouse mode when holding it down.");

			Instance = this;

			try
			{
				Harmony.CreateAndPatchAll(typeof(PlayerRotaterPlugin));
			}
			catch (System.Exception ex)
			{
				Logger.LogError($"Failed to apply patch: {ex}");
			}
		}

		private void RotatePlayer(float pitch = 0f, float roll = 0f, float yaw = 0f)
		{
			PlayerAvatarTransform.Rotate(new Vector3(pitch, yaw, roll));
		}

		// A patch to handle mouse mode.
		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPostfix]
		public static void InputPatch()
		{
			Instance.OnUpdateInput();
		}

		private void OnUpdateInput()
		{
			// Don't touch rotations if CVR doesn't want us to currently.
			if (!ABI_RC.Core.Player.PlayerSetup.Instance._movementSystem.canRot) return;

			// Only use rotation whilst flying.
			if (!ABI_RC.Core.Player.PlayerSetup.Instance._movementSystem.flying)
			{
				MouseLookEnabled = EnabledState.Off;
				if (originalRotation is not null)
				{
					PlayerAvatarTransform.eulerAngles = new Vector3(
						originalRotation.Value.x,
						PlayerAvatarTransform.eulerAngles.y,
						originalRotation.Value.z
					);
					originalRotation = null;
				}
				return;
			}

			if (originalRotation is null) originalRotation = PlayerAvatarTransform.eulerAngles;

			ProcessMouseLookState();

			// After this everything is for when mouse look is enabled.
			if (MouseLookEnabled == EnabledState.Off) return;

			RotatePlayer(
				pitch: ABI_RC.Core.Savior.CVRInputManager.Instance.lookVector.y,
				roll: ABI_RC.Core.Savior.CVRInputManager.Instance.lookVector.x * -1
			);

			ABI_RC.Core.Savior.CVRInputManager.Instance.lookVector = Vector2.zero;
		}

		private void ProcessMouseLookState()
		{
			if (mouseModeToggleKeybind.Value.IsDown())
			{
				if (MouseLookEnabled == EnabledState.Toggled) MouseLookEnabled = EnabledState.Off;
				else MouseLookEnabled = EnabledState.Toggled;
			}
			else if (Input.GetKey(mouseModeHoldKey.Value)) MouseLookEnabled = EnabledState.Holding;
			else if (MouseLookEnabled == EnabledState.Holding) MouseLookEnabled = EnabledState.Off;
		}
	}
}
