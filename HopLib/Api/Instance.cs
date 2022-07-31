using System;
using HarmonyLib;
using Instancing = ABI_RC.Core.Networking.IO.Instancing;
using MetaPort = ABI_RC.Core.Savior.MetaPort;
using NetworkManager = ABI_RC.Core.Networking.NetworkManager;

namespace HopLib
{
	public static partial class HopApi
	{
		/// <summary>The current instance's ID.</summary>
		/// <remarks>If you only need this on instance change, prefer using <see cref="InstanceJoined" />.</remarks>
		public static string CurrentInstanceId
		{
			get
			{
				return MetaPort.Instance.CurrentInstanceId;
			}
		}

		/// <summary>Invoked when the local player has started the process of joining another instance. One of the only reliable ways to retrieve the instance ID.</summary>
		public static event EventHandler<InstanceEventArgs> InstanceJoiningStarted = delegate { };

		/// <summary>Invoked when the local player has joined a new instance, but the world is probably not loaded fully yet.</summary>
		public static event EventHandler InstanceJoined = delegate { };

		/// <summary>Invoked when the local player has disconnected from the game network.</summary>
		public static event EventHandler InstanceDisconnect = delegate { };

		[HarmonyPatch(typeof(Instancing.Instances), nameof(Instancing.Instances.SetJoinTarget))]
		[HarmonyPostfix]
		private static void OnInstanceJoiningStartPatch(string __0)
		{
#if DEBUG
			HopLibPlugin.GetLogger().LogInfo($"Invoking {nameof(InstanceJoiningStarted)} {__0}");
#endif
			InstanceJoiningStarted.Invoke(null, new InstanceEventArgs(__0));
		}

		internal static void InvokeDisconnect()
		{
#if DEBUG
			HopLibPlugin.GetLogger()
				.LogInfo($"Invoking {nameof(InstanceDisconnect)}");
#endif
			InstanceDisconnect.Invoke(null, null);
		}

		[HarmonyPatch(typeof(NetworkManager), nameof(NetworkManager.OnGameNetworkConnected))]
		[HarmonyPostfix]
		private static void OnInstanceJoinedPatch()
		{
#if DEBUG
			HopLibPlugin.GetLogger().LogInfo($"Invoking {nameof(InstanceJoined)}");
#endif
			InstanceJoined.Invoke(null, null);
		}
	}

	/// <summary>Arguments for an instance related event.</summary>
	/// <remarks>Used for example in <see cref="HopApi.InstanceJoined" />.</remarks>
	public class InstanceEventArgs : EventArgs
	{
		internal InstanceEventArgs(string instanceId)
		{
			InstanceId = instanceId;
		}

		/// <summary>The instance's ID that this event relates to.</summary>
		public string InstanceId;
	}
}
