using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

class EmoteState
{
	public float Emote;

	public EmoteState(float emote)
	{
		Emote = emote;
	}
}

namespace KeyRebinder
{
	class EmotePatches
	{
		public static bool IsModifierPressed()
		{
			if (EmoteModifier.Value == KeyCode.None) return false;
			return Input.GetKey(EmoteModifier.Value);
		}

		private static ConfigEntry<bool> ExcludeGestureModifiers;
		private static ConfigEntry<bool> ExcludeStateModifier;
		private static ConfigEntry<KeyCode> EmoteModifier;
		private static ConfigEntry<KeyCode> Emote1;
		private static ConfigEntry<KeyCode> Emote2;
		private static ConfigEntry<KeyCode> Emote3;
		private static ConfigEntry<KeyCode> Emote4;
		private static ConfigEntry<KeyCode> Emote5;
		private static ConfigEntry<KeyCode> Emote6;
		private static ConfigEntry<KeyCode> Emote7;
		private static ConfigEntry<KeyCode> Emote8;

		public static void RegisterConfigs(ConfigFile Config)
		{
			ExcludeGestureModifiers = Config.Bind(
				nameof(EmotePatches),
				"ExcludeGestureModifiers",
				true,
				"If to not trigger this when pressing down the gesture modifier keys");
			ExcludeStateModifier = Config.Bind(
					nameof(EmotePatches),
					"ExcludeStateModifier",
					true,
					"If to not trigger this when pressing down the state modifier key");
			EmoteModifier = Config.Bind(
				nameof(EmotePatches),
				"BindMod",
				KeyCode.None,
				"The modifier for emotes");
			Emote1 = Config.Bind(
				nameof(EmotePatches),
				"Bind1",
				KeyCode.F1,
				"The shortcut for this emote");
			Emote2 = Config.Bind(
				nameof(EmotePatches),
				"Bind2",
				KeyCode.F2,
				"The shortcut for this emote");
			Emote3 = Config.Bind(
				nameof(EmotePatches),
				"Bind3",
				KeyCode.F3,
				"The shortcut for this emote");
			Emote4 = Config.Bind(
				nameof(EmotePatches),
				"Bind4",
				KeyCode.F4,
				"The shortcut for this emote");
			Emote5 = Config.Bind(
				nameof(EmotePatches),
				"Bind5",
				KeyCode.F5,
				"The shortcut for this emote");
			Emote6 = Config.Bind(
				nameof(EmotePatches),
				"Bind6",
				KeyCode.F6,
				"The shortcut for this emote");
			Emote7 = Config.Bind(
				nameof(EmotePatches),
				"Bind7",
				KeyCode.F7,
				"The shortcut for this emote");
			Emote8 = Config.Bind(
				nameof(EmotePatches),
				"Bind8",
				KeyCode.F8,
				"The shortcut for this emote");
		}

		public static void Patch()
		{
			Harmony.CreateAndPatchAll(typeof(EmotePatches));
		}

		private static float? EmoteNumberKeyFloat()
		{
			if (Input.GetKeyDown(Emote1.Value)) return 1f;
			if (Input.GetKeyDown(Emote2.Value)) return 2f;
			if (Input.GetKeyDown(Emote3.Value)) return 3f;
			if (Input.GetKeyDown(Emote4.Value)) return 4f;
			if (Input.GetKeyDown(Emote5.Value)) return 5f;
			if (Input.GetKeyDown(Emote6.Value)) return 6f;
			if (Input.GetKeyDown(Emote7.Value)) return 7f;
			if (Input.GetKeyDown(Emote8.Value)) return 8f;
			return null;
		}

		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPrefix]
		static void GetEmoteValues(ref EmoteState __state)
		{
			// Storing these in the Harmony Emote so that other functions can still modify the values,
			// But any changes made by Game code during UpdateInput are overridden by the Postfix.
			__state = new EmoteState(ABI_RC.Core.Savior.CVRInputManager.Instance.emote);
		}


		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPostfix]
		static void SetEmoteValue(ref EmoteState __state)
		{
			// If the current modifier is none or pressed, while excluding other modifiers if required
			if (
				(EmoteModifier.Value == KeyCode.None || IsModifierPressed()) &&
				!(ExcludeGestureModifiers.Value && GesturePatches.IsModifierPressed()) &&
				!(ExcludeStateModifier.Value && StatePatches.IsModifierPressed())
				)
			{
				var valFloat = EmoteNumberKeyFloat();
				if (valFloat.HasValue) __state.Emote = valFloat.Value;
			}

			ABI_RC.Core.Savior.CVRInputManager.Instance.emote = __state.Emote;
		}
	}
}
