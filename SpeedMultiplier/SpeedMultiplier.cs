using BepInEx;
using BepInEx.Configuration;
using HopLib;
using Player = ABI_RC.Core.Player;

namespace SpeedMultiplier
{
	[BepInDependency(HopLibInfo.GUID, HopLibInfo.Version)]
	[BepInPlugin(BuildInfo.GUID, BuildInfo.Name, BuildInfo.Version)]
	[BepInProcess("ChilloutVR.exe")]
	class SpeedMultiplierPlugin : BaseUnityPlugin
	{
		private ConfigEntry<float> FlyingMultiplier;

		public void Awake()
		{
			FlyingMultiplier = Config.Bind(
				"Speeds",
				"Flying",
				5f,
				"The flying speed multiplier. Set to <= 0 to disable (requires world reload)");

			HopApi.WorldStarted += delegate { ProcessUpdate(); };
			FlyingMultiplier.SettingChanged += delegate { ProcessUpdate(); };
		}

		private void ProcessUpdate()
		{
#if DEBUG
			Logger.LogInfo($"{nameof(FlyingMultiplier)}: {FlyingMultiplier.Value}");
#endif
			if (FlyingMultiplier.Value > 0)
				SetFlySpeedMultiplier(FlyingMultiplier.Value);
		}

		private void SetFlySpeedMultiplier(float speed)
		{
#if DEBUG
			Logger.LogInfo($"Setting flying speed to {speed}");
#endif
			Player.CVR_MovementSystem.Instance.floatSpeedMultiplier = speed;
		}
	}
}
