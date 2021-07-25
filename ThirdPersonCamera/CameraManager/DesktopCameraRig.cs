using UnityEngine;
using HarmonyLib;

namespace ThirdPersonCamera
{
	// Just moves the DesktopCameraRig
	public class DesktopCamerarigManager : CameraManager
	{
		private Harmony harmonyInstance;
		internal override GameObject TransformCameraObject
		{
			get
			{
				return ABI_RC.Core.Player.PlayerSetup.Instance.desktopCameraRig;
			}
		}

		public DesktopCamerarigManager()
		{
			MoveCamera(forwardOrBack: 5f);
			harmonyInstance = Harmony.CreateAndPatchAll(typeof(DesktopCamerarigManager));
		}

		// Resets the camera back to the original
		public override void ResetToOriginal()
		{
			harmonyInstance.UnpatchSelf();
		}

		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPostfix]
		static void InputPatches()
		{
			ABI_RC.Core.Savior.CVRInputManager.Instance.lookVector = Vector2.zero;
		}
	}
}
