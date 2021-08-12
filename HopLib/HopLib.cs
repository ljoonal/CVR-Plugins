using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;
using HarmonyLib;
using CVRWorld = ABI.CCK.Components.CVRWorld;
using CVRAssetInfo = ABI.CCK.Components.CVRAssetInfo;
using Instancing = ABI_RC.Core.Networking.IO.Instancing;

// This file contains some logic that requires keeping state.
// If you're just wanting to use the HopApi, head over to the `Api` folder instead,

namespace HopLib
{
	[BepInPlugin(HopLibInfo.GUID, HopLibInfo.Name, HopLibInfo.Version)]
	[BepInProcess("ChilloutVR.exe")]
	internal class HopLibPlugin : BaseUnityPlugin
	{
		internal static HopLibPlugin Instance;
		private static string _joinTargetId;
		internal static string InstanceId;
		internal static CVRWorld World;
		internal static string WorldId
		{
			get
			{
				return World.gameObject.GetComponent<CVRAssetInfo>().guid;
			}
		}
		internal static string GameModeId = "SocialVR";

		internal static BepInEx.Logging.ManualLogSource GetLogger()
		{
			return Instance.Logger;
		}

		[HarmonyPatch(typeof(Instancing.Instances), nameof(Instancing.Instances.SetJoinTarget))]
		[HarmonyPostfix]
		static void JoinStartPatch(string __0, string __1)
		{
#if DEBUG
			Instance.Logger.LogInfo($"Instance change, new target is {__0}, world {__1}");
#endif
			_joinTargetId = __0;
			// Because there is no good way to retrieve this yet, just placeholder.
			GameModeId = "SocialVR";
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{

			CVRWorld world = FindObjectOfType<CVRWorld>();
			if (world == null || world.gameObject.scene != scene)
			{
#if DEBUG
			Instance.Logger.LogInfo($"Non-world scene loaded: {scene.name} (mode {mode})");
#endif
				return;
			}
#if DEBUG
			Instance.Logger.LogInfo($"CVR world scene loaded: {scene.name} (mode {mode})");
#endif

			World = world;
			InstanceId = _joinTargetId;

			if (WorldId is null || WorldId == "")
			{
#if DEBUG
			Instance.Logger.LogWarning($"Not invoking InstanceJoined, worldId is empty");
#endif
				return;
			}

			if (InstanceId is null || InstanceId == "")
			{
#if DEBUG
				Instance.Logger.LogWarning($"Not invoking InstanceJoined, InstanceId is empty");
#endif
				return;
			}

			HopApi.InvokeInstanceJoined(new InstanceEventArgs(InstanceId, World));
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

		private void Awake()
		{
			try
			{
				SceneManager.sceneLoaded += OnSceneLoaded;
				Harmony.CreateAndPatchAll(typeof(HopLibPlugin));
				Harmony.CreateAndPatchAll(typeof(HopApi));
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
