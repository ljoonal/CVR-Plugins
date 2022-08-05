using BepInEx.Configuration;
using HarmonyLib;
using Valve.VR;
using InputManager = ABI_RC.Core.Savior.CVRInputManager;
using InputModule = ABI_RC.Core.Savior.InputModuleSteamVR;

class GestureAndRaw
{
	public LeftAndRightGesture Gesture;
	public LeftAndRightGesture Raw;
}

namespace KeyRebinder
{
	static class GestureVrPatches
	{
		private static bool locked;
		public static void RegisterConfigs(ConfigFile Config)
		{

		}

		public static void Patch()
		{
			Harmony.CreateAndPatchAll(typeof(GestureVrPatches));
		}

		[HarmonyPatch(typeof(InputModule), nameof(InputModule.UpdateInput))]
		[HarmonyPrefix]
		static void GetGestureValues(InputModule __instance, ref GestureAndRaw __state)
		{
			var gestureToggle = Traverse.Create(__instance).Field<SteamVR_Action_Boolean>("steamVrIndexGestureToggle");
			if (gestureToggle.Value.stateDown)
			{
				try
				{
					// cba to replace the message, but can at least say "enabled"/"disabled" correctly.
					Traverse.Create(__instance).Field<bool>("_steamVrIndexGestureToggleValue").Value = locked;
				}
				catch { }

				locked = !locked;
			}
			// Storing these in the Harmony state so that other functions can still modify the values,
			// But any changes made by Game code during UpdateInput are overridden by the Postfix.
			__state = new GestureAndRaw
			{
				Raw = new LeftAndRightGesture
				{
					Left = InputManager.Instance.gestureLeftRaw,
					Right = InputManager.Instance.gestureRightRaw
				},
				Gesture = new LeftAndRightGesture
				{
					Left = InputManager.Instance.gestureLeft,
					Right = InputManager.Instance.gestureRight
				}
			};
		}


		[HarmonyPatch(typeof(InputModule), nameof(InputModule.UpdateInput))]
		[HarmonyPostfix]
		static void SetGestureValue(ref GestureAndRaw __state)
		{
			if (InputManager.Instance.individualFingerTracking != true)
				InputManager.Instance.individualFingerTracking = true;

			if (locked)
			{
				InputManager.Instance.gestureLeftRaw = __state.Raw.Left;
				InputManager.Instance.gestureRightRaw = __state.Raw.Right;
				InputManager.Instance.gestureLeft = __state.Gesture.Left;
				InputManager.Instance.gestureRight = __state.Gesture.Right;
			}
		}
	}
}
