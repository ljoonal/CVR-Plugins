using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;
using DownloadJob = ABI_RC.Core.IO.DownloadJob;
using CVRObjectLoader = ABI_RC.Core.IO.CVRObjectLoader;
using CVRPortalManager = ABI_RC.Core.InteractionSystem.CVRPortalManager;

namespace MALogger
{
	[BepInPlugin(BuildInfo.GUID, BuildInfo.Name, BuildInfo.Version)]
	[BepInProcess("ChilloutVR.exe")]
	public class MALoggerPlugin : BaseUnityPlugin
	{
		private static MALoggerPlugin Instance;
		private ConfigEntry<bool> PropLoggingEnabled, AvatarLoggingEnabled, PortalLoggingEnabled;

		public void Awake()
		{
			const string settingsCategory = "Settings";
			PropLoggingEnabled = Config.Bind(
				settingsCategory,
				"LogProps",
				true,
				"If to log props");
			AvatarLoggingEnabled = Config.Bind(
				settingsCategory,
				"LogAvatars",
				true,
				"If to log avatars");
			PortalLoggingEnabled = Config.Bind(
				settingsCategory,
				"LogPortals",
				true,
				"If to dropped portals avatars");

			Instance = this;

			try
			{
				Harmony.CreateAndPatchAll(typeof(MALoggerPlugin));
			}
			catch (System.Exception ex)
			{
				Logger.LogError($"Failed to apply patch: {ex}");
			}
		}

		[HarmonyPatch(typeof(CVRObjectLoader), "InstantiateAvatar")]
		[HarmonyPostfix]
		public static void OnAvatarLoadPatch(string objectId, string instTarget)
		{
			if (!Instance.AvatarLoggingEnabled.Value) return;
			string targetText = instTarget != null ? " for " + instTarget : "";
			Instance.Logger.LogInfo($"Avatar {objectId}{targetText}");
		}

		[HarmonyPatch(typeof(CVRObjectLoader), "InstantiateProp")]
		[HarmonyPostfix]
		public static void OnPropLoadPatch(string objectId, string instTarget)
		{
			if (!Instance.PropLoggingEnabled.Value) return;
			string targetText = instTarget != null ? " for " + instTarget : "";
			Instance.Logger.LogInfo($"Prop {objectId}{targetText}");
		}

		// Patch a bit later so that the data is initialized properly.
		[HarmonyPatch(typeof(CVRPortalManager), "WriteData")]
		[HarmonyPostfix]
		public static void OnPortalDropPatch(CVRPortalManager __instance)
		{
			if (!Instance.PortalLoggingEnabled.Value || __instance.type != CVRPortalManager.PortalType.Instance) return;
			Instance.Logger.LogInfo($"Portal to {__instance.GetInstanceId} ({__instance.GetWorldId}) by {__instance.portalOwner}");
		}
	}
}
