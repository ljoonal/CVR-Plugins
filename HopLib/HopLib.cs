using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;
using HarmonyLib;
using CVRObjectLoader = ABI_RC.Core.IO.CVRObjectLoader;
using Instancing = ABI_RC.Core.Networking.IO.Instancing;

namespace HopLib
{
	[BepInPlugin(HopLibInfo.GUID, HopLibInfo.Name, HopLibInfo.Version)]
	[BepInProcess("ChilloutVR.exe")]
	internal class HopLibPlugin : BaseUnityPlugin
	{
		internal static HopLibPlugin Instance;
		private static string _joinTargetId;
		internal static string InstanceId;
		internal static ABI.CCK.Components.CVRWorld World;
		internal static string WorldId
		{
			get
			{
				return World.gameObject.GetComponent<ABI.CCK.Components.CVRAssetInfo>().guid;
			}
		}
		internal static string GameModeId = "SocialVR";

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

			ABI.CCK.Components.CVRWorld world = GameObject.FindObjectOfType<ABI.CCK.Components.CVRWorld>();
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

#if DEBUG
			Instance.Logger.LogInfo($"Invoking InstanceJoined: {InstanceId}. {WorldId}");
#endif

			HopApi.InvokeInstanceJoined();
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
