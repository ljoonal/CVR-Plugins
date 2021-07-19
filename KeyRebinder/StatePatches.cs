using UnityEngine;
using HarmonyLib;
using BepInEx.Configuration;

class ToggleState
{
	public float State;

	public ToggleState(float state)
	{
		State = state;
	}
}

namespace KeyRebinder
{
	class StatePatches
	{
		public static bool IsModifierPressed()
		{
			if (StateModifier.Value == KeyCode.None) return false;
			return Input.GetKey(StateModifier.Value);
		}

		private static ConfigEntry<bool> ExcludeGestureModifiers;
		private static ConfigEntry<bool> ExcludeEmoteModifier;
		private static ConfigEntry<KeyCode> StateModifier;
		private static ConfigEntry<KeyCode> DefaultState;
		private static ConfigEntry<KeyCode> State1;
		private static ConfigEntry<KeyCode> State2;
		private static ConfigEntry<KeyCode> State3;
		private static ConfigEntry<KeyCode> State4;
		private static ConfigEntry<KeyCode> State5;
		private static ConfigEntry<KeyCode> State6;
		private static ConfigEntry<KeyCode> State7;

		public static void RegisterConfigs(ConfigFile Config)
		{
			ExcludeGestureModifiers = Config.Bind(
				nameof(StatePatches),
				"ExcludeGestureModifiers",
				false,
				"If to not trigger this when pressing down the gesture modifier keys");
			ExcludeEmoteModifier = Config.Bind(
					nameof(StatePatches),
					"ExcludeEmoteModifier",
					false,
					"If to not trigger this when pressing down the emote modifier key");
			StateModifier = Config.Bind(
				nameof(StatePatches),
				"BindMod",
				KeyCode.LeftControl,
				"The modifier for states");
			DefaultState = Config.Bind(
				nameof(StatePatches),
				"BindDefaultState",
				KeyCode.F1,
				"The key for this state");
			State1 = Config.Bind(
				nameof(StatePatches),
				"Bind1",
				KeyCode.F2,
				"The key for this state");
			State2 = Config.Bind(
				nameof(StatePatches),
				"Bind2",
				KeyCode.F3,
				"The key for this state");
			State3 = Config.Bind(
				nameof(StatePatches),
				"Bind3",
				KeyCode.F4,
				"The key for this state");
			State4 = Config.Bind(
				nameof(StatePatches),
				"Bind4",
				KeyCode.F5,
				"The key for this state");
			State5 = Config.Bind(
				nameof(StatePatches),
				"Bind5",
				KeyCode.F6,
				"The key for this state");
			State6 = Config.Bind(
				nameof(StatePatches),
				"Bind6",
				KeyCode.F7,
				"The key for this state");
			State7 = Config.Bind(
				nameof(StatePatches),
				"Bind7",
				KeyCode.F8,
				"The key for this state");
		}

		public static void Patch()
		{
			Harmony.CreateAndPatchAll(typeof(StatePatches));
		}

		private static float? StateNumberKeyFloat()
		{
			if (Input.GetKeyDown(DefaultState.Value)) return 0f;
			if (Input.GetKeyDown(State1.Value)) return 1f;
			if (Input.GetKeyDown(State2.Value)) return 2f;
			if (Input.GetKeyDown(State3.Value)) return 3f;
			if (Input.GetKeyDown(State4.Value)) return 4f;
			if (Input.GetKeyDown(State5.Value)) return 5f;
			if (Input.GetKeyDown(State6.Value)) return 6f;
			if (Input.GetKeyDown(State7.Value)) return 7f;
			return null;
		}

		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPrefix]
		static void GetStateValues(ref ToggleState __state)
		{
			// Storing these in the Harmony state so that other functions can still modify the values,
			// But any changes made by Game code during UpdateInput are overridden by the Postfix.
			__state = new ToggleState(ABI_RC.Core.Savior.CVRInputManager.Instance.toggleState);
		}


		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPostfix]
		static void SetStateValue(ref ToggleState __state)
		{
			// If the current modifier is none or pressed, while excluding other modifiers if required
			if (
				(StateModifier.Value == KeyCode.None || IsModifierPressed()) &&
				!(ExcludeGestureModifiers.Value && GesturePatches.IsModifierPressed()) &&
				!(ExcludeEmoteModifier.Value && EmotePatches.IsModifierPressed())
				)
			{
				var valFloat = StateNumberKeyFloat();
				if (valFloat.HasValue) __state.State = valFloat.Value;
			}

			ABI_RC.Core.Savior.CVRInputManager.Instance.toggleState = __state.State;
		}
	}
}
