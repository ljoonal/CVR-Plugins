using BepInEx;
using BepInEx.Configuration;

namespace ColorCustomizer
{

	[BepInPlugin(BuildInfo.GUID, BuildInfo.Name, BuildInfo.Version)]
	[BepInProcess("ChilloutVR.exe")]
	public class ColorCustomizerPlugin : BaseUnityPlugin
	{
		private const string EnabledFeatures = "EnabledFeatures";
		private static ConfigEntry<bool> EnableNameplates;

		void Awake()
		{
			RegisterEnabledFeatures();

			try // MiscPatches
			{
				NameplatePatches.RegisterConfigs(Config);
				if (EnableNameplates.Value)
				{
					NameplatePatches.Patch();
					Logger.LogInfo($"{nameof(NameplatePatches)} applied");
				}
			}
			catch (System.Exception ex)
			{
				Logger.LogError($"{nameof(NameplatePatches)} failed: {ex}");
			}
		}

		private void RegisterEnabledFeatures()
		{
			EnableNameplates = Config.Bind(
				EnabledFeatures,
				"EnableFeatureNameplates",
				true,
				"If to apply nameplate patches. Requires a restart to take effect.");
		}
	}
}
