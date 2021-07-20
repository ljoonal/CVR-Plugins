using HarmonyLib;
using UnityEngine;
using BepInEx.Configuration;

namespace KeyRebinder
{
	class MicPatches
	{
		private static ConfigEntry<KeyCode> ConfigKeybindMic;
		private static ConfigEntry<KeyCode> ConfigKeybindMicPushToTalk;
		// Only null when UpdateInput has not run yet.
		private static bool? WasPushToTalkEnabled;

		public static void RegisterConfigs(ConfigFile Config)
		{
			ConfigKeybindMic = Config.Bind(
				nameof(MicPatches),
				"Bind",
				KeyCode.V,
				"The key for muting/unmuting and push to talk.");
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


			if (WasPushToTalkEnabled is null) WasPushToTalkEnabled = ABI_RC.Core.Player.InputManager.Instance.pushToTalk;
			if (Input.GetKeyDown(ConfigKeybindMicPushToTalk.Value))
			{
				WasPushToTalkEnabled = ABI_RC.Core.Player.InputManager.Instance.pushToTalk;
				ABI_RC.Core.Player.InputManager.Instance.pushToTalk = true;
				isMuteDown = true;
			}
			else if (Input.GetKeyUp(ConfigKeybindMicPushToTalk.Value))
			{
				// To turn off the mic, we need the mute button to be down here too.
				isMuteDown = true;
				ABI_RC.Core.Player.InputManager.Instance.pushToTalk = WasPushToTalkEnabled.Value;
			}
			else if (
				Input.GetKey(ConfigKeybindMicPushToTalk.Value) ||
				(
					ABI_RC.Core.Player.InputManager.Instance.pushToTalk ?
					Input.GetKey(ConfigKeybindMic.Value) :
					Input.GetKeyDown(ConfigKeybindMic.Value)
				)
			)
			{
				isMuteDown = true;
			}

			ABI_RC.Core.Savior.CVRInputManager.Instance.mute = isMuteDown;
			ABI_RC.Core.Savior.CVRInputManager.Instance.muteDown = isMuteDown;
		}
	}
}
