using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using HopLib;
using CVRPlayerManager = ABI_RC.Core.Player.CVRPlayerManager;
using CVRPortalManager = ABI_RC.Core.InteractionSystem.CVRPortalManager;

namespace MALogger
{
	[BepInDependency(HopLibInfo.GUID, HopLibInfo.Version)]
	[BepInPlugin(BuildInfo.GUID, BuildInfo.Name, BuildInfo.Version)]
	[BepInProcess("ChilloutVR.exe")]
	public class MALoggerPlugin : BaseUnityPlugin
	{
		private static MALoggerPlugin Instance;
		private ConfigEntry<bool> PropLoggingEnabled, AvatarLoggingEnabled, PortalLoggingEnabled, ConnectionLoggingEnabled;

		private readonly Dictionary<string, string> ConnectedPlayerNames = new();

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
			ConnectionLoggingEnabled = Config.Bind(
				settingsCategory,
				"LogConnections",
				true,
				"If to log join and leave messages");

			Instance = this;

			HopApi.PlayerAdded += OnPlayerJoin;
			HopApi.PlayerRemoved += OnPlayerLeave;
			HopApi.AvatarLoaded += OnAvatarLoad;
			HopApi.PropLoaded += OnPropLoad;
			HopApi.PortalLoaded += OnPortalLoad;
			HopApi.InstanceDisconnect += delegate { ConnectedPlayerNames.Clear(); };
		}

		private void OnPlayerLeave(object sender, PlayerIdEventArgs ev)
		{
			if (!Instance.ConnectionLoggingEnabled.Value) return;
			string leaverId = ev.PlayerId;
			string leaverName = ConnectedPlayerNames[leaverId];
			Instance.Logger.LogInfo($"Player left: {leaverName} ({leaverId})");
			ConnectedPlayerNames.Remove(leaverId);
		}

		private void OnPlayerJoin(object sender, PlayerEventArgs ev)
		{
			if (!Instance.ConnectionLoggingEnabled.Value) return;
			string joinerId = ev.Player.Uuid;
			string joinerName = ev.Player.PlayerDescriptor.userName;
			Instance.Logger.LogInfo($"Player joined: {joinerName} ({joinerId})");
			ConnectedPlayerNames[joinerId] = joinerName;
		}

		private void OnAvatarLoad(object sender, AvatarEventArgs ev)
		{
			if (!Instance.AvatarLoggingEnabled.Value) return;
			string avatarGuid = ev.Avatar.GetComponent<ABI.CCK.Components.CVRAssetInfo>().guid;
			string username = ev.Target?.userName ?? ABI_RC.Core.Savior.MetaPort.Instance.username;
			string userId = ev.Target?.ownerId ?? ABI_RC.Core.Savior.MetaPort.Instance.ownerId;
			Instance.Logger.LogInfo($"Avatar: {avatarGuid} by {username} ({userId})");
		}

		private void OnPropLoad(object sender, PropEventArgs ev)
		{
			if (!Instance.PropLoggingEnabled.Value) return;
			string spawnerId = ev.Prop.SpawnedBy;
			string spawnerName = CVRPlayerManager.Instance.TryGetPlayerName(spawnerId);
			Instance.Logger.LogInfo($"Prop: {ev.Prop.Spawnable.guid} by {spawnerName} ({spawnerId})");
		}

		private void OnPortalLoad(object sender, PortalEventArgs ev)
		{
			if (!Instance.PortalLoggingEnabled.Value || ev.Portal.type != CVRPortalManager.PortalType.Instance) return;
			string dropperId = ev.Portal.portalOwner;
			string dropperName = CVRPlayerManager.Instance.TryGetPlayerName(dropperId);
			Instance.Logger.LogInfo($"Portal: {ev.Portal.Portal.PortalName} by {dropperName} ({dropperId})");
		}
	}
}
