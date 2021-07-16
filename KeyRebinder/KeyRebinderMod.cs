using UnityEngine;
using System;
using BepInEx;
using BepInEx.Configuration;

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
		private ConfigEntry<KeyboardShortcut> ConfigMuteButton;

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
			Logger.LogInfo("Keybind is set to: " + ConfigMuteButton.Value);
		}

		void Update()
		{
			//Mouse3 - upper side button, Mouse4 - downed side button
			if (ConfigMuteButton.Value.IsDown())
			{
				Logger.LogInfo("Keybind is down");
				ABI_RC.Core.Savior.CVRInputManager.Instance.muteDown = true;
			}
		}
	}
}
