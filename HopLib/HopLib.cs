using HarmonyLib;
using NetworkManager = ABI_RC.Core.Networking.NetworkManager;

// This file contains some logic that requires keeping state.
// If you're just wanting to use the HopApi, head over to the `Api` folder instead,

namespace HopLib
{
#if BepInEx
	[BepInEx.BepInPlugin(HopLibInfo.Id, HopLibInfo.Name, HopLibInfo.Version)]
	[BepInEx.BepInProcess("ChilloutVR.exe")]

	internal class LoadedHopLib : BepInEx.BaseUnityPlugin
#else
	internal class LoadedHopLib : MelonLoader.MelonMod
#endif
	{
		private static LoadedHopLib Instance;


		internal static void LogInfo(object data)
		{
#if BepInEx
			Instance.Logger.LogInfo(data);
#else
Instance.LoggerInstance.Msg(data);
#endif
		}

		internal static void LogError(object data)
		{
#if BepInEx
			Instance.Logger.LogInfo(data);
#else
Instance.LoggerInstance.Msg(data);
#endif
		}

		private LoadedHopLib()
		{
			if (Instance is not null)
			{
				string errMsg = "This plugin should only have a single instance";
				LogError(errMsg);
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

#if BepInEx
		private void Awake()
#else
		private override void OnApplicationStart()
#endif
		{
			try
			{
#if BepInEx
				Harmony.CreateAndPatchAll(typeof(HopApi));
#else
HarmonyInstance.PatchAll(typeof(HopApi));
#endif
				HopApi.WorldStarted += LateInit;
#if DEBUG
				LogInfo($"{nameof(LoadedHopLib)} started successfully");
#endif
			}
			catch (System.Exception ex)
			{
				LogError($"Failed to apply patch: {ex}");
			}
		}
	}
}
