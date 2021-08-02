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
		private GameObject playerAvatar
		{
			get
			{
				return ABI_RC.Core.RootLogic.Instance.localPlayerAvatarObject;
			}
		}

		private EnabledState _mouseLookEnabledField;
		private EnabledState mouseLookEnabled
		{
			get
			{
				return _mouseLookEnabledField;
			}
			set
			{
				// TODO; Reset player rotation when turned off.
				if (_mouseLookEnabledField != value)
					Logger.LogInfo($"Mouse look set to {value}");
				_mouseLookEnabledField = value;
			}
		}

		void Awake()
		{
			const string inputPrefsCategory = "Inputs";

			mouseModeToggleKeybind = Config.Bind(
				inputPrefsCategory,
				"KeybindMouseMode",
				new KeyboardShortcut(KeyCode.F, new KeyCode[] { KeyCode.LeftControl }),
				"The keybind for toggling mouse mode");

			mouseModeHoldKey = Config.Bind(
				inputPrefsCategory,
				"KeycodeMouseModeHold",
				KeyCode.None,
				"A key that enables mouse mode when holding it down.");

			Instance = this;

			try
			{
				Harmony.CreateAndPatchAll(typeof(PlayerRotaterPlugin));
				Logger.LogInfo("Started successfully!");
			}
			catch (System.Exception ex)
			{
				Logger.LogError($"Failed to apply patch: {ex}");
			}
		}

		private void RotatePlayer(float pitch = 0f, float roll = 0f, float yaw = 0f)
		{
			playerAvatar.transform.Rotate(new Vector3(pitch, yaw, roll));
		}

		// A patch to handle mouse mode.
		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPostfix]
		static void InputPatches()
		{
			// Only allow rotation whilst flying.
			if (!ABI_RC.Core.Player.PlayerSetup.Instance._movementSystem.flying)
			{
				Instance.mouseLookEnabled = EnabledState.Off;
				return;
			}

			if (Instance.mouseModeToggleKeybind.Value.IsDown())
			{
				if (Instance.mouseLookEnabled == EnabledState.Toggled)
					Instance.mouseLookEnabled = EnabledState.Off;
				else Instance.mouseLookEnabled = EnabledState.Toggled;
			}
			else if (Input.GetKey(Instance.mouseModeHoldKey.Value))
				Instance.mouseLookEnabled = EnabledState.Holding;
			else if (Instance.mouseLookEnabled == EnabledState.Holding)
				Instance.mouseLookEnabled = EnabledState.Off;

			// After this everything is for when mouse look is enabled.
			if (Instance.mouseLookEnabled == EnabledState.Off) return;

			Instance.RotatePlayer(
				pitch: ABI_RC.Core.Savior.CVRInputManager.Instance.lookVector.y,
				yaw: ABI_RC.Core.Savior.CVRInputManager.Instance.lookVector.x
			);

			ABI_RC.Core.Savior.CVRInputManager.Instance.lookVector = Vector2.zero;
		}
	}
}
