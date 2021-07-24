using UnityEngine;

namespace ThirdPersonCamera
{
	public class ThirdPersonCameraManager
	{
		private GameObject OurCamera;

		public ThirdPersonCameraManager()
		{
			ABI_RC.Core.Player.PlayerSetup.Instance.desktopCamera.GetComponent<Camera>().enabled = false;

			var parent = ABI_RC.Core.Player.PlayerSetup.Instance.desktopCamera.transform;

			OurCamera = new GameObject("ThirdPersonCamera");
			OurCamera.tag = "MainCamera";
			OurCamera.AddComponent<Camera>();
			OurCamera.GetComponent<Camera>().fieldOfView = 75f;
			OurCamera.transform.parent = parent.transform;
			OurCamera.transform.rotation = parent.transform.rotation;
			OurCamera.transform.position = parent.transform.position;

			OurCamera.SetActive(true);
			OurCamera.GetComponent<Camera>().enabled = true;

			MoveCamera(forwardOrBack: 5f);
		}

		public void EnsureLookingAtOriginalViewpoint()
		{
			// TODO; Fix camera being sideways when looking directly up or down.
			OurCamera.transform.LookAt(ABI_RC.Core.Player.PlayerSetup.Instance.desktopCamera.transform.position);
		}

		// Resets the camera back to the original
		public void ResetToOriginal()
		{
			OurCamera.GetComponent<Camera>().enabled = false;
			OurCamera.SetActive(false);
			GameObject.Destroy(OurCamera);
			ABI_RC.Core.Player.PlayerSetup.Instance.desktopCamera.GetComponent<Camera>().enabled = true;
		}

		public void MoveCamera(float sideways = 0f, float upOrDown = 0f, float forwardOrBack = 0f)
		{
			// TODO: Learn Quaternions to replace this jank.
			if (forwardOrBack != 0f)
				OurCamera.transform.localPosition += new Vector3(0f, 0f, forwardOrBack);
			if (sideways != 0f)
				OurCamera.transform.RotateAround(
					ABI_RC.Core.Player.PlayerSetup.Instance.desktopCamera.transform.position, Vector3.up, sideways
					);
			if (upOrDown != 0f)
				OurCamera.transform.RotateAround(
					ABI_RC.Core.Player.PlayerSetup.Instance.desktopCamera.transform.position, Vector3.forward, upOrDown
					);
		}
	}
}
