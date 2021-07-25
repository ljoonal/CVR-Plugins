using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;

namespace ThirdPersonCamera
{
	[BepInPlugin(BuildInfo.GUID, BuildInfo.Name, BuildInfo.Version)]
	[BepInProcess("ChilloutVR.exe")]
	public class ThirdPersonCameraMod : BaseUnityPlugin
	{
		private static ThirdPersonCameraMod Instance;
		private static ConfigEntry<KeyboardShortcut> KeybindToggle;
		private static ConfigEntry<KeyCode> KeybindCameraMove;

#nullable enable
		private static OurCameraManager? OurCameraManagerInstance = null;
#nullable disable

		public static BepInEx.Logging.ManualLogSource GetLogger()
		{
			return Instance.Logger;
		}

		public static KeyCode GetCameraMoveKeyCode()
		{
			return KeybindCameraMove.Value;
		}

		void Awake()
		{
			KeybindToggle = Config.Bind(
				"Settings",
				"KeybindToggle",
				new KeyboardShortcut(KeyCode.T, new KeyCode[] { KeyCode.LeftControl }),
				"The keybind for toggling third person mode");
			KeybindCameraMove = Config.Bind(
				"Settings",
				"KeybindCameraMoveMode",
				KeyCode.LeftControl,
				"The keybind for moving the camera around around whilst holding it down.");
		}

		void Update()
		{
			if (KeybindToggle.Value.IsDown())
			{
				if (OurCameraManagerInstance is null) OurCameraManagerInstance = new OurCameraManager();
				else
				{
					OurCameraManagerInstance.ResetToOriginal();
					OurCameraManagerInstance = null;
				}
			}

			if (OurCameraManagerInstance is not null)
			{
				var sideways = Input.GetKey(KeybindCameraMove.Value) ? Input.GetAxis("Mouse X") : 0f;
				var upOrDown = Input.GetKey(KeybindCameraMove.Value) ? Input.GetAxis("Mouse Y") : 0f;
				var forwardOrBack = Input.GetAxis("Mouse ScrollWheel");


				OurCameraManagerInstance.EnsureLookingAtOriginalViewpoint();
				if (forwardOrBack != 0f || sideways != 0f || upOrDown != 0f)
					OurCameraManagerInstance.MoveCamera(sideways, upOrDown, forwardOrBack);
			}
		}
	}
}
