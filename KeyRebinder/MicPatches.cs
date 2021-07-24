using HarmonyLib;
using UnityEngine;
using BepInEx.Configuration;

namespace KeyRebinder
{
	class MicPatches
	{
		private static ConfigEntry<KeyCode> ConfigKeybindMicToggle;
		private static ConfigEntry<KeyCode> ConfigKeybindMicPushToTalk;

		public static void RegisterConfigs(ConfigFile Config)
		{
			ConfigKeybindMicToggle = Config.Bind(
				nameof(MicPatches),
				"BindToggle",
				KeyCode.V,
				"The key for toggling mute");
			ConfigKeybindMicPushToTalk = Config.Bind(
				nameof(MicPatches),
					"BindPushToTalk",
					KeyCode.Mouse3,
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

			if (Input.GetKeyDown(ConfigKeybindMicPushToTalk.Value))
			{
				ABI_RC.Core.Player.InputManager.Instance.pushToTalk = true;
				isMuteDown = true;
			}
			else if (Input.GetKeyUp(ConfigKeybindMicPushToTalk.Value))
			{
				// To turn off the mic, we need the mute button to be down here too.
				isMuteDown = true;
				ABI_RC.Core.Player.InputManager.Instance.pushToTalk = false;
			}
			else if (
				Input.GetKey(ConfigKeybindMicPushToTalk.Value) ||
				Input.GetKeyDown(ConfigKeybindMicToggle.Value)
			)
			{
				isMuteDown = true;
			}

			ABI_RC.Core.Savior.CVRInputManager.Instance.mute = isMuteDown;
			ABI_RC.Core.Savior.CVRInputManager.Instance.muteDown = isMuteDown;
		}
	}
}
