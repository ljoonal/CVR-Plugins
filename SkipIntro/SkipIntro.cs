using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkipIntro
{
	[BepInPlugin(BuildInfo.Id, BuildInfo.Name, BuildInfo.Version)]
	[BepInProcess("ChilloutVR.exe")]
	public class SkipIntroPlugin : BaseUnityPlugin
	{
		private ConfigEntry<bool> Enabled;
		private bool Skipped;
		public void Awake()
		{
			Enabled = Config.Bind(
				"Settings",
				"Enabled",
				true,
				"If to skip the intro or not.");

			if (Enabled.Value) TrySkipIntro();
			if (!Skipped) SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			TrySkipIntro();
		}

		private void TrySkipIntro()
		{
			if (Skipped) return;
			ABI_RC.Core.Savior.SkipIntro skipIntro = Component.FindObjectOfType<ABI_RC.Core.Savior.SkipIntro>();
			if (skipIntro is not null)
			{
				SceneManager.LoadSceneAsync(1);
				SceneManager.sceneLoaded -= OnSceneLoaded;
				Skipped = true;
			}
		}
	}
}
