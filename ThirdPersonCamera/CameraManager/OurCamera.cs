using UnityEngine;
using HarmonyLib;

namespace ThirdPersonCamera
{
	// Uses an extra camera.
	public class OurCameraManager : CameraManager
	{
		private GameObject _camera;
		private Harmony harmonyInstance;
		internal override GameObject TransformCameraObject
		{
			get
			{
				return _camera;
			}
		}

		public OurCameraManager()
		{
			ABI_RC.Core.Player.PlayerSetup.Instance.desktopCamera.GetComponent<Camera>().enabled = false;

			var parent = ABI_RC.Core.Player.PlayerSetup.Instance.desktopCamera.transform;

			_camera = new GameObject("ThirdPersonCamera");
			TransformCameraObject.tag = "MainCamera";
			TransformCameraObject.AddComponent<Camera>();
			TransformCameraObject.GetComponent<Camera>().fieldOfView = 75f;
			TransformCameraObject.transform.parent = parent.transform;
			TransformCameraObject.transform.rotation = parent.transform.rotation;
			TransformCameraObject.transform.position = parent.transform.position;

			TransformCameraObject.SetActive(true);
			TransformCameraObject.GetComponent<Camera>().enabled = true;

			MoveCamera(forwardOrBack: 5f);

			harmonyInstance = Harmony.CreateAndPatchAll(typeof(OurCameraManager));
		}

		public override void ResetToOriginal()
		{
			TransformCameraObject.GetComponent<Camera>().enabled = false;
			TransformCameraObject.SetActive(false);
			harmonyInstance.UnpatchSelf();
			GameObject.Destroy(TransformCameraObject);
			ABI_RC.Core.Player.PlayerSetup.Instance.desktopCamera.GetComponent<Camera>().enabled = true;
		}

		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPostfix]
		static void InputPatches()
		{
			if (Input.GetKey(ThirdPersonCameraMod.GetCameraMoveKeyCode()))
				ABI_RC.Core.Savior.CVRInputManager.Instance.lookVector = Vector2.zero;
		}
	}
}
