using BepInEx.Configuration;
using System.Reflection;

namespace SpeedMultiplier
{
	class FlyingPatches
	{
		private static ConfigEntry<bool> PatchEnabled;
		private static ConfigEntry<float> FlyingMultiplier;

		public static void Setup(ConfigFile Config)
		{
			float defaultFlySpeed = 5f;
			PatchEnabled = Config.Bind(
				nameof(FlyingPatches),
				"Enabled",
				true,
				"If the flying speed patch is enabled or not. Requires restart to take effect.");
			FlyingMultiplier = Config.Bind(
				nameof(FlyingPatches),
				"Speed",
				defaultFlySpeed,
				"The flying speed multiplier");

			FlyingMultiplier.SettingChanged += new System.EventHandler(OnSettingsChanged);

			if (PatchEnabled.Value) SetFlySpeedMultiplier(FlyingMultiplier.Value);
		}

		private static void OnSettingsChanged(object sender, System.EventArgs e)
		{
			if (PatchEnabled.Value) SetFlySpeedMultiplier(FlyingMultiplier.Value);
		}

		private static async void SetFlySpeedMultiplier(float speed)
		{
			if (ABI_RC.Core.Player.PlayerSetup.Instance is null)
			{
				await System.Threading.Tasks.Task.Delay(1000);
				SetFlySpeedMultiplier(speed);
			}
			else
			{
				typeof(ABI_RC.Core.Player.CVR_MovementSystem).GetField("floatSpeedMultiplier", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(ABI_RC.Core.Player.PlayerSetup.Instance._movementSystem, speed);
			}
		}
	}
}
