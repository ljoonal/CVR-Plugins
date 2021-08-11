using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;
using CVRObjectLoader = ABI_RC.Core.IO.CVRObjectLoader;
using Instancing = ABI_RC.Core.Networking.IO.Instancing;
using System;

namespace HopLib
{
	/// <summary>The main Hop API.</summary>
	/// <remarks>
	/// The Hop API's purpose is to help modders.
	/// It will eventually have events and utilities for commonly required things.
	/// Currently it is under heavy development, so expect breaking changes even with minor revisions before the 1.0.0 release
	/// </remarks>
	public class HopApi
	{
		/// <summary>The current game mode's ID.</summary>
		/// <remarks>Currently it'll just a static default, but in the future this will be implemented if possible.</remarks>
		public static string CurrentGameModeId
		{
			get
			{
				return HopLibPlugin.GameModeId;
			}
		}

		/// <summary>The current instance's ID.</summary>
		public static string CurrentInstanceId
		{
			get
			{
				return HopLibPlugin.InstanceId;
			}
		}

		/// <summary>The current world's ID.</summary>
		public static string CurrentWorldId
		{
			get
			{
				return HopLibPlugin.WorldId;
			}
		}

		/// <summary>Invoked when the local player has joined a new instance.</summary>
		public static event EventHandler InstanceJoined = delegate { };
		internal static void InvokeInstanceJoined()
		{
			InstanceJoined.Invoke(null, null);
		}
	}

}
