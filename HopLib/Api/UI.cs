using System;
using HarmonyLib;
using ViewManager = ABI_RC.Core.InteractionSystem.ViewManager;

namespace HopLib
{
	public static partial class HopApi
	{
		/// <summary>The moment when UI injections should happen.</summary>
		public static event EventHandler<InjectUiEventArgs> UIEventsRegistered = delegate { };

		[HarmonyPatch(typeof(ViewManager), nameof(ViewManager.RegisterEvents))]
		[HarmonyPostfix]
		private static void InjectScripts(ViewManager __instance)
		{
			foreach (Delegate handler in UIEventsRegistered.GetInvocationList())
			{
				try
				{
					handler.DynamicInvoke(null, new InjectUiEventArgs(__instance));
				}
				catch (Exception ex)
				{
					LoadedHopLib.LogError($"UIEventsRegistered's handler {handler} threw an exception: {ex}");
				}
			}
		}
	}

	/// <summary>Arguments to an UI event.</summary>
	/// <remarks>Used for example in <see cref="HopApi.UIEventsRegistered" />.</remarks>
	public class InjectUiEventArgs : EventArgs
	{
		internal InjectUiEventArgs(ViewManager viewManager)
		{
			Manager = viewManager;
		}

		/// <summary>The ID of the player that the event relates to.</summary>
		public ViewManager Manager;
	}
}
