using UnityEngine;
using HarmonyLib;

namespace ThirdPersonCamera
{
	// The basic third person camera manager.
	public abstract class CameraManager
	{
		internal abstract GameObject TransformCameraObject { get; }

		public void EnsureLookingAtOriginalViewpoint()
		{
			// TODO; Fix camera being sideways when looking directly up or down.
			TransformCameraObject.transform.LookAt(ABI_RC.Core.Player.PlayerSetup.Instance.desktopCamera.transform.position);
		}

		// Resets the camera back to the original
		public abstract void ResetToOriginal();

		public void MoveCamera(float sideways = 0f, float upOrDown = 0f, float forwardOrBack = 0f)
		{
			EnsureLookingAtOriginalViewpoint();
			// TODO: Learn Quaternions to replace this jank.
			if (forwardOrBack != 0f)
				TransformCameraObject.transform.position += (TransformCameraObject.transform.forward * forwardOrBack);
			if (sideways != 0f)
				TransformCameraObject.transform.RotateAround(
					ABI_RC.Core.Player.PlayerSetup.Instance.desktopCamera.transform.position, Vector3.up, sideways
					);
			if (upOrDown != 0f)
				TransformCameraObject.transform.RotateAround(
					ABI_RC.Core.Player.PlayerSetup.Instance.desktopCamera.transform.position, Vector3.forward, upOrDown
					);
		}
	}
}
