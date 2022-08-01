using System.Linq;
using UnityEngine;

#if BepInEx
using BepInEx.Configuration;
#endif

namespace HopLib.Extras
{
#if MelonLoader
	public class KeyboardShortcut
	{
		public KeyCode MainKey;
		public KeyCode[] Modifiers;
		// Taken with LGPL2.1 license from BepInEx repo
		public KeyboardShortcut(KeyCode mainKey, KeyCode[] modifiers) {
			if (mainKey == KeyCode.None && modifiers.Any())
				throw new System.ArgumentException($"Can't set {nameof(mainKey)} to KeyCode.None if there are any {nameof(modifiers)}");
			MainKey = mainKey;
			Modifiers = modifiers;
		}
	}
#endif

	/// <summary>
	/// `KeyboardShortcut` but allows other keys to be pressed at the same time.
	/// </summary>
	public static class AllowingKeyboardShortcut
	{
		/// <summary>
		/// Check if the main key was just pressed (Input.GetKeyDown), and specified modifier keys are all pressed, while also allowing other keys to be pressed.
		/// </summary>
		public static bool AllowingIsDown(this KeyboardShortcut keyboardShortcut)
		{
			if (keyboardShortcut.MainKey == KeyCode.None) return false;

			return Input.GetKeyDown(keyboardShortcut.MainKey) &&
				keyboardShortcut.Modifiers.All(c => Input.GetKey(c));
		}

		/// <summary>
		/// Check if the main key is currently held down (Input.GetKey), and specified modifier keys are all pressed, while also allowing other keys to be pressed.
		/// </summary>
		public static bool AllowingIsPressed(this KeyboardShortcut keyboardShortcut)
		{
			if (keyboardShortcut.MainKey == KeyCode.None) return false;

			return Input.GetKey(keyboardShortcut.MainKey) &&
				keyboardShortcut.Modifiers.All(c => Input.GetKey(c));
		}

		/// <summary>
		/// Check if the main key was just lifted (Input.GetKeyUp), and specified modifier keys are all pressed, while also allowing other keys to be pressed.
		/// </summary>
		public static bool AllowingIsUp(this KeyboardShortcut keyboardShortcut)
		{
			if (keyboardShortcut.MainKey == KeyCode.None) return false;

			return Input.GetKeyUp(keyboardShortcut.MainKey) &&
				keyboardShortcut.Modifiers.All(c => Input.GetKey(c));
		}
	}
}
