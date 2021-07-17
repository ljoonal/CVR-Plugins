using UnityEngine;
using System;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace KeyRebinder
{

	public static class BuildInfo
	{
		public const string GUID = "xyz.ljoonal.cvrmods.keyrebinder";

		public const string Name = "Key Rebinder";

		public const string Version = "0.1.0";
	}

	[BepInPlugin(BuildInfo.GUID, BuildInfo.Name, BuildInfo.Version)]
	[BepInProcess("ChilloutVR.exe")]
	public class KeyRebinderMod : BaseUnityPlugin
	{
		private const string KeybindsCategory = "Keybinds";
		private static ConfigEntry<KeyboardShortcut> ConfigMuteButton;

		KeyRebinderMod()
		{
			ConfigMuteButton = Config.Bind(
				KeybindsCategory,
				"MuteButton",
				new KeyboardShortcut(KeyCode.Mouse3),
				"The key for muting/unmuting");
		}


		void Awake()
		{
			try
			{
				Harmony.CreateAndPatchAll(typeof(KeyRebinderMod));
				Logger.LogInfo("Patched keybinds successfully");
			}
			catch (Exception ex)
			{
				Logger.LogError(ex.ToString());
			}
		}


		//ABI_RC.Core.Savior.CVRInputManager.Instance.mute = true;
		//ABI_RC.Core.Savior.CVRInputManager.Instance.muteDown = true;
		// ABI_RC.Core.Base.Audio.SetMicrophoneActive(true);
		// ABI_RC.Core.Base.Audio.ToggleMicrophone();

		[HarmonyPatch(typeof(ABI_RC.Core.Player.InputManager), "Update")]
		[HarmonyPrefix]
		static void MicMute()
		{
			// Setting these only in the Harmony patch prefix,
			// because otherwise the game input handling will overwrite our changes on Update
			ABI_RC.Core.Savior.CVRInputManager.Instance.mute = ConfigMuteButton.Value.IsDown();
			ABI_RC.Core.Savior.CVRInputManager.Instance.muteDown = ConfigMuteButton.Value.IsDown();
		}
	}
}
