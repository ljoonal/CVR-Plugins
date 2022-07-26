using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

class LeftAndRightGesture
{
	public float Left;
	public float Right;
}

namespace KeyRebinder
{
	class GesturePatches
	{
		public static bool IsModifierPressed()
		{
			return (GestureLeftModifier.Value != KeyCode.None && Input.GetKey(GestureLeftModifier.Value))
				|| (GestureRightModifier.Value != KeyCode.None && Input.GetKey(GestureRightModifier.Value));
		}

		private static ConfigEntry<bool> ExcludeEmoteModifier;
		private static ConfigEntry<bool> ExcludeStateModifier;
		private static ConfigEntry<KeyCode> GestureLeftModifier;
		private static ConfigEntry<KeyCode> GestureRightModifier;
		private static ConfigEntry<KeyCode> Gesture1;
		private static ConfigEntry<KeyCode> Gesture2;
		private static ConfigEntry<KeyCode> Gesture3;
		private static ConfigEntry<KeyCode> Gesture4;
		private static ConfigEntry<KeyCode> Gesture5;
		private static ConfigEntry<KeyCode> Gesture6;
		private static ConfigEntry<KeyCode> Gesture7;
		private static ConfigEntry<KeyCode> Gesture8;

		public static void RegisterConfigs(ConfigFile Config)
		{
			ExcludeEmoteModifier = Config.Bind(
				nameof(GesturePatches),
				"ExcludeEmoteModifier",
				false,
				"If to not trigger this when pressing down the emote modifier key");
			ExcludeStateModifier = Config.Bind(
					nameof(GesturePatches),
					"ExcludeStateModifier",
					false,
					"If to not trigger this when pressing down the state modifier key");
			GestureLeftModifier = Config.Bind(
				nameof(GesturePatches),
				"BindLeftMod",
				KeyCode.LeftShift,
				"The left hand modifier for gestures");
			GestureRightModifier = Config.Bind(
				nameof(GesturePatches),
				"BindRightMod",
				KeyCode.RightShift,
				"The right hand modifier for gestures");
			Gesture1 = Config.Bind(
				nameof(GesturePatches),
				"Bind1",
				KeyCode.F1,
				"The key for this gesture");
			Gesture2 = Config.Bind(
				nameof(GesturePatches),
				"Bind2",
				KeyCode.F2,
				"The key for this gesture");
			Gesture3 = Config.Bind(
				nameof(GesturePatches),
				"Bind3",
				KeyCode.F3,
				"The key for this gesture");
			Gesture4 = Config.Bind(
				nameof(GesturePatches),
				"Bind4",
				KeyCode.F4,
				"The key for this gesture");
			Gesture5 = Config.Bind(
				nameof(GesturePatches),
				"Bind5",
				KeyCode.F5,
				"The key for this gesture");
			Gesture6 = Config.Bind(
				nameof(GesturePatches),
				"Bind6",
				KeyCode.F6,
				"The key for this gesture");
			Gesture7 = Config.Bind(
				nameof(GesturePatches),
				"Bind7",
				KeyCode.F7,
				"The key for this gesture");
			Gesture8 = Config.Bind(
				nameof(GesturePatches),
				"Bind8",
				KeyCode.F8,
				"The key for this gesture");
		}

		public static void Patch()
		{
			Harmony.CreateAndPatchAll(typeof(GesturePatches));
		}

		// For some reason the gestures are stored as floats ranging from -1 to 6...
		private static float? GestureNumberKeyFloat()
		{
			if (Input.GetKeyDown(Gesture1.Value)) return -1f;
			if (Input.GetKeyDown(Gesture2.Value)) return 0f;
			if (Input.GetKeyDown(Gesture3.Value)) return 1f;
			if (Input.GetKeyDown(Gesture4.Value)) return 2f;
			if (Input.GetKeyDown(Gesture5.Value)) return 3f;
			if (Input.GetKeyDown(Gesture6.Value)) return 4f;
			if (Input.GetKeyDown(Gesture7.Value)) return 5f;
			if (Input.GetKeyDown(Gesture8.Value)) return 6f;
			return null;
		}

		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPrefix]
		static void GetGestureValues(ref LeftAndRightGesture __state)
		{
			// Storing these in the Harmony state so that other functions can still modify the values,
			// But any changes made by Game code during UpdateInput are overridden by the Postfix.
			__state = new LeftAndRightGesture
			{
				Left = ABI_RC.Core.Savior.CVRInputManager.Instance.gestureLeft,
				Right = ABI_RC.Core.Savior.CVRInputManager.Instance.gestureRight
			};
		}


		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPostfix]
		static void SetGestureValue(ref LeftAndRightGesture __state)
		{
			// Excluding other modifiers if required first
			if (
				!(ExcludeStateModifier.Value && GesturePatches.IsModifierPressed()) &&
				!(ExcludeEmoteModifier.Value && EmotePatches.IsModifierPressed())
				)
			{
				// If the hand gesture modifer keys are being pressed.
				var updateLeftHand = GestureLeftModifier.Value == KeyCode.None || Input.GetKey(GestureLeftModifier.Value);
				var updateRightHand = GestureRightModifier.Value == KeyCode.None || Input.GetKey(GestureRightModifier.Value);
				if (updateLeftHand || updateRightHand)
				{
					// Get possible individual gesture keys.
					var valFloat = GestureNumberKeyFloat();
					if (valFloat.HasValue && updateLeftHand) __state.Left = valFloat.Value;
					if (valFloat.HasValue && updateRightHand) __state.Right = valFloat.Value;
				}
			}

			ABI_RC.Core.Savior.CVRInputManager.Instance.gestureLeft = __state.Left;
			ABI_RC.Core.Savior.CVRInputManager.Instance.gestureRight = __state.Right;
		}
	}
}
