using System;
using ABI.CCK.Components;
using HarmonyLib;

namespace HopLib
{
	public static partial class HopApi
	{
		/// <summary>The current world.</summary>
		public static CVRWorld CurrentWorld
		{
			get
			{
				return CVRWorld.Instance;
			}
		}

		/// <summary>Invoked when a world has been loaded.</summary>
		public static event EventHandler<WorldEventArgs> WorldStarted = delegate { };

		[HarmonyPatch(typeof(CVRWorld), nameof(CVRWorld.Start))]
		[HarmonyPostfix]
		private static void OnWorldStartPatch(CVRWorld __instance)
		{
#if DEBUG
			HopLibPlugin.GetLogger()
				.LogInfo($"Invoking {nameof(WorldStarted)} by {__instance?.gameObject?.GetComponent<CVRAssetInfo>()?.guid}");
#endif
			WorldStarted.Invoke(null, new WorldEventArgs(__instance));
		}
	}

	/// <summary>Arguments for a world related event.</summary>
	/// <remarks>Used for example in <see cref="HopApi.WorldStarted" />.</remarks>
	public class WorldEventArgs : EventArgs
	{
		internal WorldEventArgs(CVRWorld world)
		{
			World = world;
		}

		/// <summary>The world that the event relates to.</summary>
		public CVRWorld World;

	}
}
