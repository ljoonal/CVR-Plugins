using BepInEx.Configuration;
using BepInEx.Logging;
using UnityEngine;
using System.Reflection;

namespace SpeedMultiplier
{
	class FlyingPatches
	{
		private static float OriginalFlyingSpeedMultiplier;
		private static ConfigEntry<bool> PatchEnabled;
		private static ConfigEntry<float> FlyingMultiplier;
		private static PropertyInfo FlyingSpeedPropertyInfo;

		public static void Setup(ConfigFile Config)
		{
			FlyingSpeedPropertyInfo = typeof(ABI_RC.Core.Player.CVR_MovementSystem).GetProperty("floatSpeedMultiplier", BindingFlags.Instance | BindingFlags.NonPublic);
			OriginalFlyingSpeedMultiplier = (float)FlyingSpeedPropertyInfo.GetValue(ABI_RC.Core.Player.PlayerSetup.Instance._movementSystem);
			PatchEnabled = Config.Bind(
				nameof(FlyingPatches),
				"Enabled",
				true,
				"If the flying speed patch is enabled or not.");
			FlyingMultiplier = Config.Bind(
				nameof(FlyingPatches),
				"Speed",
				5f,
				"The flying speed multiplier");

			PatchEnabled.SettingChanged += new System.EventHandler(OnSettingsChanged);

			PatchOrUnpatch();
		}

		private static void OnSettingsChanged(object sender, System.EventArgs e)
		{
			PatchOrUnpatch();
		}

		private static void PatchOrUnpatch()
		{
			if (PatchEnabled.Value) SetFlySpeedMultiplier(FlyingMultiplier.Value);
			else SetFlySpeedMultiplier(OriginalFlyingSpeedMultiplier);
		}

		private static void SetFlySpeedMultiplier(float speed)
		{
			FlyingSpeedPropertyInfo.SetValue(ABI_RC.Core.Player.PlayerSetup.Instance._movementSystem, speed);
		}
	}
}
