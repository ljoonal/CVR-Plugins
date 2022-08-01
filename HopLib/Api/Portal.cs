using System;
using HarmonyLib;
using CVRPortalManager = ABI_RC.Core.InteractionSystem.CVRPortalManager;


namespace HopLib
{
	public static partial class HopApi
	{
		/// <summary>Invoked when a portal has been loaded.</summary>
		public static event EventHandler<PortalEventArgs> PortalLoaded = delegate { };

		[HarmonyPatch(typeof(CVRPortalManager), nameof(CVRPortalManager.WriteData))]
		[HarmonyPostfix]
		private static void OnPortalDropPatch(CVRPortalManager __instance)
		{
#if DEBUG
			LoadedHopLib.LogInfo($"Invoking {nameof(PortalLoaded)} by {__instance.portalOwner} to {__instance.Portal.PortalName}");
#endif
			PortalLoaded.Invoke(null, new PortalEventArgs(__instance));
		}
	}

	/// <summary>Arguments for a portal related event.</summary>
	/// <remarks>Used for example in <see cref="HopApi.PortalLoaded" />.</remarks>
	public class PortalEventArgs : EventArgs
	{
		internal PortalEventArgs(CVRPortalManager portal)
		{
			Portal = portal;
		}

		/// <summary>The avatar that the event relates to.</summary>
		public CVRPortalManager Portal;
	}
}
