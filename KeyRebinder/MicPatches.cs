using HarmonyLib;
using UnityEngine;
using BepInEx.Configuration;

namespace KeyRebinder
{
	class MicPatches
	{
		private static ConfigEntry<KeyCode> ConfigKeybindMic;

		public static void RegisterConfigs(ConfigFile Config)
		{
			ConfigKeybindMic = Config.Bind(
				nameof(MicPatches),
				"Bind",
				KeyCode.Mouse3,
				"The key for muting/unmuting and push to talk.");
		}

		public static void Patch()
		{
			Harmony.CreateAndPatchAll(typeof(MicPatches));
		}

		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPostfix]
		static void OverwriteInputs()
		{
			// Setting these only in the Harmony patch prefix,
			// because otherwise the game input handling will overwrite our changes on Update

			// using only GetKey seems a bit glitchy in toggle to talk, so use GetKeyDown in that mode.
			var isMuteDown = ABI_RC.Core.Player.InputManager.Instance.pushToTalk ?
				Input.GetKey(ConfigKeybindMic.Value) :
				Input.GetKeyDown(ConfigKeybindMic.Value);
			ABI_RC.Core.Savior.CVRInputManager.Instance.mute = isMuteDown;
			ABI_RC.Core.Savior.CVRInputManager.Instance.muteDown = isMuteDown;
		}
	}
}
