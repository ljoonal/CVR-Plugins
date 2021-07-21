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
		private static ConfigEntry<KeyboardShortcut> ConfigKeybindNameplatesToggle;
		private static ConfigEntry<KeyboardShortcut> ConfigKeybindHudToggle;

		public static void RegisterConfigs(ConfigFile Config)
		{
			ConfigKeybindReload = Config.Bind(
				nameof(MiscPatches),
				"BindReload",
				new KeyboardShortcut(KeyCode.F9, new KeyCode[] { KeyCode.LeftControl }),
				"The shortcut for reloading (Set to None to let the game manage it)");
			ConfigKeybindSwitchMode = Config.Bind(
				nameof(MiscPatches),
				"BindSwitchMode",
				new KeyboardShortcut(KeyCode.F9, new KeyCode[] { KeyCode.LeftShift }),
				"The shortcut for switching mode (Set to None to let the game manage it)");
			ConfigKeybindNameplatesToggle = Config.Bind(
				nameof(MiscPatches),
				"BindNameplateToggle",
				new KeyboardShortcut(KeyCode.None),
				"The shortcut for toggling nameplate visibility. (Set to None to let the game manage it)");
			ConfigKeybindHudToggle = Config.Bind(
				nameof(MiscPatches),
				"BindHudToggle",
				new KeyboardShortcut(KeyCode.None),
				"The shortcut for toggling hud visibility. (Set to None to let the game manage it)");
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
			// because otherwise the game input handling may overwrite our changes on Update
			if (ConfigKeybindReload.Value.MainKey != KeyCode.None)
				ABI_RC.Core.Savior.CVRInputManager.Instance.reload = ConfigKeybindReload.Value.IsDown();
			if (ConfigKeybindSwitchMode.Value.MainKey != KeyCode.None)
				ABI_RC.Core.Savior.CVRInputManager.Instance.switchMode = ConfigKeybindSwitchMode.Value.IsDown();
			if (ConfigKeybindNameplatesToggle.Value.MainKey != KeyCode.None)
				ABI_RC.Core.Savior.CVRInputManager.Instance.toggleNameplates = ConfigKeybindNameplatesToggle.Value.IsDown();
			if (ConfigKeybindHudToggle.Value.MainKey != KeyCode.None)
				ABI_RC.Core.Savior.CVRInputManager.Instance.toggleHud = ConfigKeybindHudToggle.Value.IsDown();
		}
	}
}
