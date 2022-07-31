using System;

namespace HopLib
{
	/// <summary>The main Hop API.</summary>
	/// <remarks>
	/// The Hop API's purpose is to help modders.
	/// It will eventually have events and utilities for commonly required things.
	/// Currently it is under heavy development, so expect breaking changes even with minor revisions before the 1.0.0 release
	/// </remarks>
	/// <example><code>
	/// using HopLib;
	/// HopApi.PlayerAdded += (object sender, PlayerEventArgs ev) => {
	///   Logger.LogInfo($"Player {ev.Player.Username} added.");
	/// };
	/// </code></example>
	public static partial class HopApi
	{
		// The api is split accross the whole Api folder, split into more logical sections.

		/// <summary>Invoked only once, sometime after the game and HopLib have intialized their data.</summary>
		public static event EventHandler LateInit = delegate { };

		internal static void InvokeLateInit()
		{
#if DEBUG
			HopLibPlugin.GetLogger()
				.LogInfo($"Invoking {nameof(LateInit)}");
#endif
			LateInit.Invoke(null, null);
		}
	}
}
