using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;

namespace KeyRebinder
{
	class MovementPatches
	{
		private static ConfigEntry<KeyCode> ConfigJump;
		private static ConfigEntry<KeyCode> ConfigSprint;
		private static ConfigEntry<KeyCode> ConfigCrouch;
		private static ConfigEntry<KeyCode> ConfigProne;
		private static ConfigEntry<KeyCode> ConfigFlyToggle;
		private static ConfigEntry<KeyCode> ConfigFlyUp;
		private static ConfigEntry<KeyCode> ConfigFlyDown;
		private static bool AllowFlyingToggle = true;

		public static void RegisterConfigs(ConfigFile Config)
		{
			ConfigJump = Config.Bind(
				nameof(MovementPatches),
				"BindJump",
				KeyCode.Space,
				"The keybind for jumping");
			ConfigSprint = Config.Bind(
				nameof(MovementPatches),
				"BindSprint",
				KeyCode.LeftShift,
				"The keybind for sprinting");
			ConfigCrouch = Config.Bind(
				nameof(MovementPatches),
				"BindCrouch",
				KeyCode.C,
				"The keybind for crouching");
			ConfigProne = Config.Bind(
				nameof(MovementPatches),
				"BindProne",
				KeyCode.X,
				"The keybind for going prone");
			ConfigFlyToggle = Config.Bind(
				nameof(MovementPatches),
				"BindFlyToggle",
				KeyCode.Keypad5,
				"The keybind for toggling flying");
			ConfigFlyUp = Config.Bind(
				nameof(MovementPatches),
				"BindFlyUp",
				KeyCode.Space,
				"The keybind for flying up");
			ConfigFlyDown = Config.Bind(
				nameof(MovementPatches),
				"BindFlyDown",
				KeyCode.C,
				"The keybind for flying down");
		}

		public static void Patch()
		{
			Harmony.CreateAndPatchAll(typeof(MovementPatches));
		}

		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPrefix]
		static void PreUpdateInputValues(ref float __state)
		{
			__state = ABI_RC.Core.Savior.CVRInputManager.Instance.floatDirection;
		}

		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPostfix]
		static void OverwriteInputs(ref float __state)
		{
			// Setting these only in the Harmony patch prefix,
			// because otherwise the game input handling will overwrite our changes on Update

			ABI_RC.Core.Savior.CVRInputManager.Instance.crouchToggle = Input.GetKeyDown(ConfigCrouch.Value);
			ABI_RC.Core.Savior.CVRInputManager.Instance.proneToggle = Input.GetKeyDown(ConfigProne.Value);
			ABI_RC.Core.Savior.CVRInputManager.Instance.jump = Input.GetKey(ConfigJump.Value);
			ABI_RC.Core.Savior.CVRInputManager.Instance.sprint = Input.GetKey(ConfigSprint.Value);

			if (Input.GetKey(ConfigFlyUp.Value)) __state += 1f;
			if (Input.GetKey(ConfigFlyDown.Value)) __state -= 1f;

			ABI_RC.Core.Savior.CVRInputManager.Instance.floatDirection = __state;
		}

		[HarmonyPatch(typeof(ABI_RC.Core.IO.CVRPathCamController), "Update")]
		[HarmonyPrefix]
		static void FlyingToggling()
		{
			if (Input.GetKeyDown(ConfigFlyToggle.Value))
			{
				ABI_RC.Core.Player.PlayerSetup.Instance._movementSystem.toggleFlight();
			}
			AllowFlyingToggle = false;
		}

		[HarmonyPatch(typeof(ABI_RC.Core.IO.CVRPathCamController), "Update")]
		[HarmonyPostfix]
		static void FlyingTogglingAfter()
		{
			// Re-enable flying after the code that has a hotkey for it has run.
			AllowFlyingToggle = true;
		}

		[HarmonyPatch(typeof(ABI_RC.Core.Player.CVR_MovementSystem), "toggleFlight")]
		[HarmonyPrefix]
		static bool LimitFlying()
		{
			// Only allow our own fn to call this function to toggle flying.
			return AllowFlyingToggle;
		}
	}
}
