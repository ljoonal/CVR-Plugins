using BepInEx;
using BepInEx.Configuration;
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

			ForwardOrBack(5f);
		}

		private void EnsureLookingAtOriginalViewpoint()
		{
			// TODO; Fix camera being sideways when looking directly up or down.
			OurCamera.transform.LookAt(ABI_RC.Core.Player.PlayerSetup.Instance.desktopCamera.transform.position);
		}

		// Resets the camera back to the original
		public void ResetToOriginal()
		{
			GameObject.Destroy(OurCamera);
			ABI_RC.Core.Player.PlayerSetup.Instance.desktopCamera.GetComponent<Camera>().enabled = true;
		}

		public void ForwardOrBack(float forwardOrBack)
		{
			OurCamera.transform.localPosition += new Vector3(0f, 0f, forwardOrBack);
			EnsureLookingAtOriginalViewpoint();
		}
	}
}
