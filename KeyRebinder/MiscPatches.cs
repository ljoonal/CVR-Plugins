using HarmonyLib;
using BepInEx.Configuration;
using UnityEngine;

namespace KeyRebinder
{
	// TODO: Headturn
	class MiscPatches
	{
		private static ConfigEntry<KeyboardShortcut> ConfigKeybindReload;
		private static ConfigEntry<KeyboardShortcut> ConfigKeybindSwitchMode;

		public static void RegisterConfigs(ConfigFile Config)
		{
			ConfigKeybindReload = Config.Bind(
				nameof(MiscPatches),
				"BindReload",
				new KeyboardShortcut(KeyCode.F9, new KeyCode[] { KeyCode.LeftControl }),
				"The shortcut for reloading");
			ConfigKeybindSwitchMode = Config.Bind(
				nameof(MiscPatches),
				"BindSwitchMode",
				new KeyboardShortcut(KeyCode.F9, new KeyCode[] { KeyCode.LeftShift }),
				"The shortcut for switching mode..? (Probably admin related?)");
		}

		public static void Patch()
		{
			Harmony.CreateAndPatchAll(typeof(MiscPatches));
		}

		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPostfix]
		static void OverwriteInputs()
		{
			// Setting these only in the Harmony patch prefix,
			// because otherwise the game input handling will overwrite our changes on Update

			ABI_RC.Core.Savior.CVRInputManager.Instance.reload = ConfigKeybindReload.Value.IsDown();
			ABI_RC.Core.Savior.CVRInputManager.Instance.switchMode = ConfigKeybindSwitchMode.Value.IsDown();
		}
	}
}
