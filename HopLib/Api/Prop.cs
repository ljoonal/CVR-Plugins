using System;
using HarmonyLib;
using CVRSyncHelper = ABI_RC.Core.Util.CVRSyncHelper;

namespace HopLib
{
	public static partial class HopApi
	{
		/// <summary>Invoked when a prop has been loaded.</summary>
		/// <remarks>Not invoked for blocked props.</remarks>
		public static event EventHandler<PropEventArgs> PropLoaded = delegate { };

		[HarmonyPatch(typeof(CVRSyncHelper), nameof(CVRSyncHelper.ApplyPropValuesSpawn))]
		[HarmonyPostfix]
		private static void OnPropLoadPatch(CVRSyncHelper.PropData __0)
		{
#if DEBUG
			HopLibPlugin.GetLogger()
				.LogInfo($"Invoking {nameof(PropLoaded)} by {__0.SpawnedBy}, prop {__0.Spawnable.guid}");
#endif
			PropLoaded.Invoke(null, new PropEventArgs(__0));
		}
	}

	/// <summary>Arguments for a prop related event.</summary>
	/// <remarks>Used for example in <see cref="HopApi.PropLoaded" />.</remarks>
	public class PropEventArgs : EventArgs
	{
		internal PropEventArgs(CVRSyncHelper.PropData prop)
		{
			Prop = prop;
		}

		/// <summary>The prop that the event relates to.</summary>
		public CVRSyncHelper.PropData Prop;
	}

}
