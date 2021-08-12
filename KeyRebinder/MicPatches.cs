using HarmonyLib;
using UnityEngine;
using BepInEx.Configuration;
using HopLib.Extras;

namespace KeyRebinder
{
	class MicPatches
	{
		private static ConfigEntry<KeyboardShortcut> ConfigKeybindMicToggle,
			ConfigKeybindMicPushToTalk;

		public static void RegisterConfigs(ConfigFile Config)
		{
			ConfigKeybindMicToggle = Config.Bind(
				nameof(MicPatches),
				"BindToggle",
				new KeyboardShortcut(KeyCode.V),
				"The key for toggling mute");
			ConfigKeybindMicPushToTalk = Config.Bind(
				nameof(MicPatches),
					"BindPushToTalk",
					new KeyboardShortcut(KeyCode.Mouse3),
					"The key that enables push to talk whilst pressing it.");
		}

		public static void Patch()
		{
			Harmony.CreateAndPatchAll(typeof(MicPatches));
		}

		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPostfix]
		static void OverwriteInputs()
		{
			var isMuteDown = false;

			if (ConfigKeybindMicPushToTalk.Value.AllowingIsDown())
			{
				ABI_RC.Core.Player.InputManager.Instance.pushToTalk = true;
				isMuteDown = true;
			}
			else if (ConfigKeybindMicPushToTalk.Value.AllowingIsUp())
			{
				// To turn off the mic, we need the mute button to be down here too.
				isMuteDown = true;
				ABI_RC.Core.Player.InputManager.Instance.pushToTalk = false;
			}
			else if (
				ConfigKeybindMicPushToTalk.Value.AllowingIsPressed() ||
				ConfigKeybindMicToggle.Value.AllowingIsDown()
			)
			{
				isMuteDown = true;
			}

			ABI_RC.Core.Savior.CVRInputManager.Instance.mute = isMuteDown;
			ABI_RC.Core.Savior.CVRInputManager.Instance.muteDown = isMuteDown;
		}
	}
}
