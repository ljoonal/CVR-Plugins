using System;
using HarmonyLib;
using UnityEngine;
using Player = ABI_RC.Core.Player;

namespace HopLib
{
	public static partial class HopApi
	{
		/// <summary>Invoked when an user's avatar has been loaded.</summary>
		public static event EventHandler<AvatarEventArgs> AvatarLoaded = delegate { };

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "IDE0051", Justification = "Harmony patch uses this")]
		[HarmonyPatch(typeof(Player.PlayerSetup), nameof(Player.PlayerSetup.SetupAvatar))]
		[HarmonyPostfix]
		private static void OnLocalAvatarLoadPatch(GameObject __0)
		{
#if DEBUG
			HopLibPlugin.GetLogger()
				.LogInfo($"Invoking {nameof(AvatarLoaded)} with local avatar {__0.GetComponent<ABI.CCK.Components.CVRAssetInfo>().guid}");
#endif
			AvatarLoaded.Invoke(null, new AvatarEventArgs(__0, null));
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "IDE0051", Justification = "Harmony patch uses this")]
		[HarmonyPatch(typeof(Player.PuppetMaster), nameof(Player.PuppetMaster.AvatarInstantiated))]
		[HarmonyPostfix]
		private static void OnRemoteAvatarLoadPatch(Player.PuppetMaster __instance)
		{
			Player.PlayerDescriptor playerDescription = Traverse.Create(__instance)
				.Field("_playerDescriptor")
				.GetValue<Player.PlayerDescriptor>();

#if DEBUG
			HopLibPlugin.GetLogger()
				.LogInfo($"Invoking {nameof(AvatarLoaded)} for {playerDescription.ownerId} with avatar {__instance.avatarObject.GetComponent<ABI.CCK.Components.CVRAssetInfo>().guid}");
#endif

			AvatarLoaded.Invoke(null, new AvatarEventArgs(__instance.avatarObject, playerDescription));
		}
	}

#nullable enable
	/// <summary>Arguments for an avatar related event.</summary>
	/// <remarks>Used for example in <see cref="HopApi.AvatarLoaded" />.</remarks>
	public class AvatarEventArgs : EventArgs
	{
		internal AvatarEventArgs(GameObject avatar, Player.PlayerDescriptor? target)
		{
			Avatar = avatar;
			Target = target;
		}

		/// <summary>The avatar that the event relates to.</summary>
		public GameObject Avatar;
		/// <summary>The player that the event is related to, null for local player.</summary>
		public Player.PlayerDescriptor? Target;

		/// <summary>If the target is the local player.</summary>
		public bool IsLocal
		{
			get
			{
				return Target == null;
			}
		}
	}
#nullable disable
}
