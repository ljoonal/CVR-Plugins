using System;
using ABI.CCK.Components;

namespace HopLib
{
	public partial class HopApi
	{
		/// <summary>The current game mode's ID.</summary>
		/// <remarks>
		/// Currently it'll just a static default, but in the future this will be implemented if possible.
		/// If you only need this on instance change, prefer using <see cref="InstanceJoined" />.
		/// </remarks>
		public static string CurrentGameModeId
		{
			get
			{
				return HopLibPlugin.GameModeId;
			}
		}

		/// <summary>The current instance's ID.</summary>
		/// <remarks>If you only need this on instance change, prefer using <see cref="InstanceJoined" />.</remarks>
		public static string CurrentInstanceId
		{
			get
			{
				return HopLibPlugin.InstanceId;
			}
		}

		/// <summary>The current world's ID.</summary>
		/// <remarks>If you only need this on instance change, prefer using <see cref="InstanceJoined" />.</remarks>
		public static string CurrentWorldId
		{
			get
			{
				return HopLibPlugin.WorldId;
			}
		}

		/// <summary>Invoked when the local player has joined a new instance.</summary>
		public static event EventHandler<InstanceEventArgs> InstanceJoined = delegate { };
		internal static void InvokeInstanceJoined(InstanceEventArgs instanceDetails)
		{
#if DEBUG
			HopLibPlugin.GetLogger().LogInfo($"Invoking {nameof(InstanceJoined)}: {instanceDetails.InstanceId} - {instanceDetails.World.name}");
#endif
			InstanceJoined.Invoke(null, instanceDetails);
		}
	}

	/// <summary>Arguments for an instance related event.</summary>
	/// <remarks>Used for example in <see cref="HopApi.InstanceJoined" />.</remarks>
	public class InstanceEventArgs : EventArgs
	{
		internal InstanceEventArgs(string instanceId, CVRWorld world)
		{
			InstanceId = instanceId;
			World = world;
		}

		/// <summary>The world that the event relates to.</summary>
		public CVRWorld World;
		/// <summary>The instance's ID that this event relates to.</summary>
		public string InstanceId;
	}
}
