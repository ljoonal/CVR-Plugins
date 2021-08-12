using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;
using CVRPlayerManager = ABI_RC.Core.Player.CVRPlayerManager;
using DownloadJob = ABI_RC.Core.IO.DownloadJob;
using CVRObjectLoader = ABI_RC.Core.IO.CVRObjectLoader;
using CVRPortalManager = ABI_RC.Core.InteractionSystem.CVRPortalManager;
using HopLib;

namespace MALogger
{
	[BepInDependency(HopLibInfo.GUID, HopLibInfo.Version)]
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

			HopApi.AvatarLoaded += OnAvatarLoad;
			HopApi.PropLoaded += OnPropLoad;
			HopApi.PortalLoaded += OnPortalLoad;
		}

		private void OnAvatarLoad(object sender, AvatarEventArgs ev)
		{
			if (!Instance.AvatarLoggingEnabled.Value) return;
			string avatarGuid = ev.Avatar.GetComponent<ABI.CCK.Components.CVRAssetInfo>().guid;
			string username = ev.Target?.userName ?? ABI_RC.Core.Savior.MetaPort.Instance.username;
			string userId = ev.Target?.ownerId ?? ABI_RC.Core.Savior.MetaPort.Instance.ownerId;
			Instance.Logger.LogInfo($"Avatar {avatarGuid} by {username} ({userId})");
		}

		private void OnPropLoad(object sender, PropEventArgs ev)
		{
			if (!Instance.PropLoggingEnabled.Value) return;
			string spawnerId = ev.Prop.SpawnedBy;
			string spawnerName = CVRPlayerManager.Instance.TryGetPlayerName(spawnerId);
			Instance.Logger.LogInfo($"Prop {ev.Prop.Spawnable.guid} by {spawnerName} ({spawnerId})");
		}

		private void OnPortalLoad(object sender, PortalEventArgs ev)
		{
			if (!Instance.PortalLoggingEnabled.Value || ev.Portal.type != CVRPortalManager.PortalType.Instance) return;
			string dropperId = ev.Portal.portalOwner;
			string dropperName = CVRPlayerManager.Instance.TryGetPlayerName(dropperId);
			Instance.Logger.LogInfo($"Portal to {ev.Portal.Portal.PortalName} by {dropperName} ({dropperId})");
		}
	}
}
