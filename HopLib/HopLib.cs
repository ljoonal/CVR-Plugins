using BepInEx;
using HarmonyLib;
using NetworkManager = ABI_RC.Core.Networking.NetworkManager;

// This file contains some logic that requires keeping state.
// If you're just wanting to use the HopApi, head over to the `Api` folder instead,

namespace HopLib
{
	[BepInPlugin(HopLibInfo.Id, HopLibInfo.Name, HopLibInfo.Version)]
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
				throw new System.InvalidOperationException(errMsg);
			}
			Instance = this;
		}

		private void LateInit(object sender, System.EventArgs e)
		{
			NetworkManager.Instance.GameNetwork.Disconnected += delegate { HopApi.InvokeDisconnect(); };
			HopApi.WorldStarted -= LateInit;
			HopApi.InvokeLateInit();
		}

		private void Awake()
		{
			try
			{
				Harmony.CreateAndPatchAll(typeof(HopApi));
				HopApi.WorldStarted += LateInit;
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
