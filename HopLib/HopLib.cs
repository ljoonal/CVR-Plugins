using BepInEx;
using HarmonyLib;
using UnityEngine.SceneManagement;
using NetworkManager = ABI_RC.Core.Networking.NetworkManager;

// This file contains some logic that requires keeping state.
// If you're just wanting to use the HopApi, head over to the `Api` folder instead,

namespace HopLib
{
	[BepInPlugin(HopLibInfo.GUID, HopLibInfo.Name, HopLibInfo.Version)]
	[BepInProcess("ChilloutVR.exe")]
	internal class HopLibPlugin : BaseUnityPlugin
	{
		private static HopLibPlugin Instance;

		internal static BepInEx.Logging.ManualLogSource GetLogger()
		{
			return Instance.Logger;
		}

		private HopLibPlugin()
		{
			if (Instance is not null)
			{
				string errMsg = "This plugin should only have a single instance";
				Logger.LogError(errMsg);
				throw new System.Exception(errMsg);
			}
			Instance = this;
		}

		private void OnceClientFullyInitialized(object sender, System.EventArgs e)
		{
			NetworkManager.Instance.GameNetwork.Disconnected += delegate { HopApi.InvokeDisconnect(); };
			HopApi.WorldStarted -= OnceClientFullyInitialized;
		}

		private void Awake()
		{
			try
			{
				Harmony.CreateAndPatchAll(typeof(HopApi));
				HopApi.WorldStarted += OnceClientFullyInitialized;
#if DEBUG
				Logger.LogInfo($"{nameof(HopLibPlugin)} started successfully");
#endif
			}
			catch (System.Exception ex)
			{
				Logger.LogError($"Failed to apply patch: {ex}");
			}
		}

	}
}
