using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using CVRPlayerEntity = ABI_RC.Core.Player.CVRPlayerEntity;
using CVRPlayerManager = ABI_RC.Core.Player.CVRPlayerManager;

namespace HopLib
{
	public static partial class HopApi
	{
		/// <summary>Invoked when a remote player has been added (when for example loading in or they connected).</summary>
		public static event EventHandler<PlayerEventArgs> PlayerAdded = delegate { };

		/// <summary>Invoked when a remote player has been removed (when for example they disconnected).</summary>
		public static event EventHandler<PlayerIdEventArgs> PlayerRemoved = delegate { };

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "IDE0051", Justification = "Harmony patch uses this")]
		[HarmonyPatch(typeof(CVRPlayerManager), nameof(CVRPlayerManager.TryCreatePlayer))]
		[HarmonyPrefix]
		private static void PlayerAddedPrefix(CVRPlayerManager __instance, ref List<string> __state)
		{
			__state = __instance.NetworkPlayers.Select(p => p.Uuid).ToList();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "IDE0051", Justification = "Harmony patch uses this")]
		[HarmonyPatch(typeof(CVRPlayerManager), nameof(CVRPlayerManager.TryCreatePlayer))]
		[HarmonyPostfix]
		private static void PlayerAddedPostfix(CVRPlayerManager __instance, List<string> __state)
		{
			foreach (var player in __instance.NetworkPlayers.Where(p => !__state.Contains(p.Uuid)))
			{
#if DEBUG
				HopLibPlugin.GetLogger()
					.LogInfo($"Invoking {nameof(PlayerAdded)} for {player.Uuid}");
#endif
				PlayerAdded.Invoke(null, new PlayerEventArgs(player));
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "IDE0051", Justification = "Harmony patch uses this")]
		[HarmonyPatch(typeof(CVRPlayerManager), nameof(CVRPlayerManager.TryDeletePlayer))]
		[HarmonyPrefix]
		private static void PlayerDeletedPrefix(CVRPlayerManager __instance, ref List<string> __state)
		{
			__state = __instance.NetworkPlayers.Select(p => p.Uuid).ToList();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "IDE0051", Justification = "Harmony patch uses this")]
		[HarmonyPatch(typeof(CVRPlayerManager), nameof(CVRPlayerManager.TryDeletePlayer))]
		[HarmonyPostfix]
		private static void PlayerDeletedPostfix(CVRPlayerManager __instance, List<string> __state)
		{
			foreach (var playerId in __state.Where(id => !__instance.NetworkPlayers.Any(p => p.Uuid == id)))
			{
#if DEBUG
				HopLibPlugin.GetLogger()
					.LogInfo($"Invoking {nameof(PlayerRemoved)} for {playerId}");
#endif
				PlayerRemoved.Invoke(null, new PlayerIdEventArgs(playerId));
			}
		}
	}

	/// <summary>Arguments for a player related event.</summary>
	/// <remarks>
	/// Used for example in <see cref="HopApi.PlayerAdded" />.
	/// If you want to access the player's avatar, you should instead use <see cref="HopApi.AvatarLoaded" />.
	/// Please note that this value might be copied via reflections to avoid the game recycling it, so some methods may not behave correctly.
	/// </remarks>
	public class PlayerEventArgs : EventArgs
	{
		internal PlayerEventArgs(CVRPlayerEntity player)
		{
			Player = player;
		}

		/// <summary>The player that the event relates to.</summary>
		public CVRPlayerEntity Player;
	}

	/// <summary>Arguments for a player related event but with only the ID of the player.</summary>
	/// <remarks>Used for example in <see cref="HopApi.PlayerRemoved" />.</remarks>
	public class PlayerIdEventArgs : EventArgs
	{
		internal PlayerIdEventArgs(string playerId)
		{
			PlayerId = playerId;
		}

		/// <summary>The ID of the player that the event relates to.</summary>
		public string PlayerId;
	}
}
