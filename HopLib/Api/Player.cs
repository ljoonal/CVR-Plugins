using System;
using HarmonyLib;
using DarkRift2Player = Dissonance.Integrations.DarkRift2.DarkRift2Player;
using NetworkPlayerType = Dissonance.NetworkPlayerType;

namespace HopLib
{
	public partial class HopApi
	{
		/// <summary>Invoked when a remote player has mostly joined, but it yet to be added to the.</summary>
		/// <remarks>Not invoked for blocked players or local player.</remarks>
		public static event EventHandler<PlayerEventArgs> PlayerJoining = delegate { };

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "IDE0051", Justification = "Harmony patch uses this")]
		[HarmonyPatch(typeof(DarkRift2Player), nameof(DarkRift2Player.SetPlayerId))]
		[HarmonyPostfix]
		private static void OnPlayerJoinPatch(NetworkPlayerType __0, string __1)
		{
			if (__0 == NetworkPlayerType.Remote)
			{
#if DEBUG
				HopLibPlugin.GetLogger()
					.LogInfo($"Invoking {nameof(PlayerJoining)} for {__1}");
#endif
				PlayerJoining.Invoke(null, new PlayerEventArgs(__1));
			}
		}
	}

	/// <summary>Arguments for an avatar related event.</summary>
	/// <remarks>
	/// Used for example in <see cref="HopApi.PlayerJoining" />.
	/// If you want to access the player's avatar, you should instead use <see cref="HopApi.AvatarLoaded" />.
	/// </remarks>
	public class PlayerEventArgs : EventArgs
	{
		internal PlayerEventArgs(string playerId)
		{
			PlayerId = playerId;
		}

		/// <summary>The player's ID that the event relates to.</summary>
		public string PlayerId;
	}
}
