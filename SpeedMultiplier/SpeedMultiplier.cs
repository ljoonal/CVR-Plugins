using BepInEx;

namespace SpeedMultiplier
{

	[BepInPlugin(BuildInfo.GUID, BuildInfo.Name, BuildInfo.Version)]
	[BepInProcess("ChilloutVR.exe")]
	public class SpeedMultiplierPlugin : BaseUnityPlugin
	{
		public void Awake()
		{
			try
			{
				FlyingPatches.Setup(Config);
			}
			catch (System.Exception ex)
			{
				Logger.LogError($"Failed to apply patch: {ex}");
			}

		}
	}
}
