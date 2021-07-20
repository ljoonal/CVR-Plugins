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
			ConfigKeybindNameplatesToggle = Config.Bind(
				nameof(MiscPatches),
				"BindNameplateToggle",
				new KeyboardShortcut(KeyCode.N, new KeyCode[] { KeyCode.LeftControl }),
				"The shortcut for toggling nameplate visibility.");
		}

		public static void Patch()
		{
			Harmony.CreateAndPatchAll(typeof(MiscPatches));
		}

		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPostfix]
		static void OverwriteInputs()
		{
			if (ConfigKeybindNameplatesToggle.Value.IsDown())
			{
				ABI_RC.Core.Savior.MetaPort.Instance.settings.SetSettingsBool(
					"GeneralShowNameplates",
					!ABI_RC.Core.Savior.MetaPort.Instance.settings.GetSettingsBool("GeneralShowNameplates")
				);
			}
			// Setting these only in the Harmony patch prefix,
			// because otherwise the game input handling may overwrite our changes on Update
			ABI_RC.Core.Savior.CVRInputManager.Instance.reload = ConfigKeybindReload.Value.IsDown();
			ABI_RC.Core.Savior.CVRInputManager.Instance.switchMode = ConfigKeybindSwitchMode.Value.IsDown();
		}
	}
}
