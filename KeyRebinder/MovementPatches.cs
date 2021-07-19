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
		}

		public static void Patch()
		{
			Harmony.CreateAndPatchAll(typeof(MovementPatches));
		}

		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPostfix]
		static void OverwriteInputs()
		{
			// Setting these only in the Harmony patch prefix,
			// because otherwise the game input handling will overwrite our changes on Update

			ABI_RC.Core.Savior.CVRInputManager.Instance.jump = Input.GetKey(ConfigJump.Value);
			ABI_RC.Core.Savior.CVRInputManager.Instance.sprint = Input.GetKey(ConfigSprint.Value);
		}
	}
}
