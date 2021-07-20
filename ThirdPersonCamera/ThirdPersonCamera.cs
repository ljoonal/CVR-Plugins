using BepInEx;
using BepInEx.Configuration;
using UnityEngine;

namespace ThirdPersonCamera
{
	[BepInPlugin(BuildInfo.GUID, BuildInfo.Name, BuildInfo.Version)]
	[BepInProcess("ChilloutVR.exe")]
	public class ThirdPersonCameraMod : BaseUnityPlugin
	{
		private static ConfigEntry<KeyboardShortcut> KeybindToggle;

#nullable enable
		private ThirdPersonCameraManager? ThirdPersonCameraManagerInstance = null;
#nullable disable
		void Awake()
		{
			KeybindToggle = Config.Bind(
				"Settings",
				"Keybind",
				new KeyboardShortcut(KeyCode.T, new KeyCode[] { KeyCode.LeftControl }),
				"The keybind for toggling third person mode");
		}

		void Update()
		{
			if (KeybindToggle.Value.IsDown())
			{
				if (ThirdPersonCameraManagerInstance is null) ThirdPersonCameraManagerInstance = new ThirdPersonCameraManager();
				else
				{
					ThirdPersonCameraManagerInstance.ResetToOriginal();
					ThirdPersonCameraManagerInstance = null;
				}
			}

			var scroll = Input.GetAxis("Mouse ScrollWheel");
			if (ThirdPersonCameraManagerInstance is not null && scroll != 0f)
			{
				ThirdPersonCameraManagerInstance.ForwardOrBack(scroll);
			}
		}
	}
}
