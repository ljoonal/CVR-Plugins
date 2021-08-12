using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;
using HopLib.Extras;

namespace ThirdPersonCamera
{
	public enum CameraState
	{
		Default,
		Front,
		Back,
		Freeform
	}

	[BepInPlugin(BuildInfo.GUID, BuildInfo.Name, BuildInfo.Version)]
	[BepInProcess("ChilloutVR.exe")]
	public class ThirdPersonCameraPlugin : BaseUnityPlugin
	{
		private static ConfigEntry<KeyboardShortcut> KeybindCycle, KeybindFrontCam, KeybindBackCam, KeybindFreeformCam, KeybindFreeformMovement;
		private static CameraState CurrentCameraState = CameraState.Default;
		private static Transform Reference
		{
			get
			{
				return ABI_RC.Core.Player.PlayerSetup.Instance.desktopCamera.transform;
			}
		}

		private static GameObject _ourCamera;

		private static GameObject CameraObject
		{
			get
			{
				if (_ourCamera is not null)
				{
					if (_ourCamera.activeSelf) return _ourCamera;
					GameObject.Destroy(_ourCamera);
				};
				// Probably cannot be retrieved instantly on game load, so we do it lazily in a getter like this.
				_ourCamera = new GameObject("ThirdPersonCamera");
				_ourCamera.AddComponent<Camera>();
				_ourCamera.GetComponent<Camera>().enabled = false;
				_ourCamera.GetComponent<Camera>().fieldOfView = 80f;
				_ourCamera.transform.position = Reference.position;

				return _ourCamera;
			}
		}

		private const string _general_preferences_category = "Settings";


		public void Awake()
		{
			KeybindCycle = Config.Bind(
				_general_preferences_category,
				"KeybindCycleCamera",
				new KeyboardShortcut(KeyCode.T, new KeyCode[] { KeyCode.LeftControl }),
				"The keybind for cycling between different camera modes (None disables)");
			KeybindFrontCam = Config.Bind(
				_general_preferences_category, "KeybindFrontCamera", new KeyboardShortcut(KeyCode.None), "The keybind used to turn on front camera mode (None disables)");
			KeybindBackCam = Config.Bind(
				_general_preferences_category, "KeybindBackCamera", new KeyboardShortcut(KeyCode.None), "The keybind used to turn on back camera mode (None disables)");
			KeybindFreeformCam = Config.Bind(
				_general_preferences_category, "KeybindFreeformCamera", new KeyboardShortcut(KeyCode.None), "The keybind used to turn on freeform camera mode (None disables)");
			KeybindFreeformMovement = Config.Bind(
				_general_preferences_category, "KeybindFreeformMovement", new KeyboardShortcut(KeyCode.Mouse1), "The keycode that enables freeform looking around whilst being held (None disables)");

			try
			{
				Harmony.CreateAndPatchAll(typeof(ThirdPersonCameraPlugin));
#if DEBUG
				Logger.LogInfo("Started successfully!");
#endif
			}
			catch (System.Exception ex)
			{
				Logger.LogError($"Failed to apply patch: {ex}");
			}
		}

		public void Update()
		{
			if (CurrentCameraState != CameraState.Default && Input.GetKeyDown(KeyCode.Escape)) UseDefaultCamera();
			else
			{
				if (KeybindCycle.Value.AllowingIsDown())
				{
					if (CurrentCameraState == CameraState.Default) ToggleFrontCamera();
					else if (CurrentCameraState == CameraState.Front) ToggleBackCamera();
					else if (CurrentCameraState == CameraState.Back) ToggleFreeformCamera();
					else UseDefaultCamera();
				}
				else if (KeybindFrontCam.Value.AllowingIsDown()) ToggleFrontCamera();
				else if (KeybindBackCam.Value.AllowingIsDown()) ToggleBackCamera();
				else if (KeybindFreeformCam.Value.AllowingIsDown()) ToggleFreeformCamera();
			}

			if (CurrentCameraState != CameraState.Default)
			{
				float scroll = Input.GetAxis("Mouse ScrollWheel");
				if (scroll != 0f) ZoomOurCamera(scroll);
			}
		}

		private void UseDefaultCamera()
		{
			CurrentCameraState = CameraState.Default;
			UpdateActiveCamera();
		}

		private void ToggleFrontCamera()
		{
			if (CurrentCameraState == CameraState.Front)
			{
				UseDefaultCamera();
				return;
			}
			CurrentCameraState = CameraState.Front;
			CameraObject.transform.parent = Reference;
			CameraObject.transform.position = Reference.position + Reference.forward;
			CameraObject.transform.LookAt(Reference);
			ZoomOurCamera(-2f);
			UpdateActiveCamera();
		}

		private void ToggleBackCamera()
		{
			if (CurrentCameraState == CameraState.Back)
			{
				UseDefaultCamera();
				return;
			}
			CurrentCameraState = CameraState.Back;
			CameraObject.transform.parent = Reference;
			CameraObject.transform.position = Reference.position - Reference.forward;
			CameraObject.transform.LookAt(Reference);
			ZoomOurCamera(-2f);
			UpdateActiveCamera();
		}

		private void ToggleFreeformCamera()
		{
			if (CurrentCameraState == CameraState.Freeform)
			{
				UseDefaultCamera();
				return;
			}
			CurrentCameraState = CameraState.Freeform;
			CameraObject.transform.parent = null;
			UpdateActiveCamera();
		}

		// Negative values 'zoom' (move) out, positive move closer.
		private void ZoomOurCamera(float forwardOrBack)
		{
			CameraObject.transform.position += (CameraObject.transform.forward * forwardOrBack);
		}

		private void UpdateActiveCamera()
		{
#if DEBUG
			Logger.LogInfo($"Toggling camera mode to: {CurrentCameraState}");
#endif
			bool useDefaultCamera = CurrentCameraState == CameraState.Default;
			CameraObject.GetComponent<Camera>().enabled = !useDefaultCamera;
			ABI_RC.Core.Player.PlayerSetup.Instance.desktopCamera.GetComponent<Camera>().enabled = useDefaultCamera;
		}

		// A patch to allow freeform mode movement using standard input methods.
		[HarmonyPatch(typeof(ABI_RC.Core.Savior.InputModuleMouseKeyboard), "UpdateInput")]
		[HarmonyPostfix]
		public static void InputPatches()
		{
			if (CurrentCameraState == CameraState.Freeform && KeybindFreeformMovement.Value.AllowingIsPressed())
			{
				CameraObject.transform.eulerAngles += new Vector3(
					ABI_RC.Core.Savior.CVRInputManager.Instance.lookVector.y * -1,
					ABI_RC.Core.Savior.CVRInputManager.Instance.lookVector.x,
					0
				);
				ABI_RC.Core.Savior.CVRInputManager.Instance.lookVector = Vector2.zero;

				var ogMovement = ABI_RC.Core.Savior.CVRInputManager.Instance.movementVector;

				Vector3 movement = new();
				movement += CameraObject.transform.up * ogMovement.y;
				movement += CameraObject.transform.right * ogMovement.x;
				movement += CameraObject.transform.forward * ogMovement.z;

				CameraObject.transform.position += movement * Time.deltaTime * 2;

				ABI_RC.Core.Savior.CVRInputManager.Instance.movementVector = Vector2.zero;
			}
		}
	}
}
