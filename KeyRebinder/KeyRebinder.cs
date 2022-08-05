using BepInEx;
using BepInEx.Configuration;
using HopLib;

namespace KeyRebinder
{
	[BepInDependency(HopLibInfo.Id, HopLibInfo.Version)]
	[BepInPlugin(BuildInfo.Id, BuildInfo.Name, BuildInfo.Version)]
	[BepInProcess("ChilloutVR.exe")]
	public class KeyRebinderPlugin : BaseUnityPlugin
	{
		private const string EnabledModules = "EnabledModules";
		private static ConfigEntry<bool> EnableModuleMic;
		private static ConfigEntry<bool> EnableModuleMisc;
		private static ConfigEntry<bool> EnableModuleMovement;
		private static ConfigEntry<bool> EnableModuleGestures;
		private static ConfigEntry<bool> EnableModuleVrGestures;
		private static ConfigEntry<bool> EnableModuleStates;
		private static ConfigEntry<bool> EnableModuleEmotes;

		void Awake()
		{
			RegisterEnabledModuleConfigs();

			try // MiscPatches
			{
				MiscPatches.RegisterConfigs(Config);
				if (EnableModuleMisc.Value)
				{
					MiscPatches.Patch();
#if DEBUG
					Logger.LogInfo($"{nameof(MiscPatches)} applied");
#endif
				}
			}
			catch (System.Exception ex)
			{
				Logger.LogError($"{nameof(MiscPatches)} failed: {ex}");
			}

			try // MicPatches
			{
				MicPatches.RegisterConfigs(Config);
				if (EnableModuleMic.Value)
				{
					MicPatches.Patch();
#if DEBUG
					Logger.LogInfo($"{nameof(MicPatches)} applied");
#endif
				}
			}
			catch (System.Exception ex)
			{
				Logger.LogError($"{nameof(MicPatches)} failed: {ex}");
			}

			try // MovementPatches
			{
				MovementPatches.RegisterConfigs(Config);
				if (EnableModuleMovement.Value)
				{
					MovementPatches.Patch();
#if DEBUG
					Logger.LogInfo($"{nameof(MovementPatches)} applied");
#endif
				}
			}
			catch (System.Exception ex)
			{
				Logger.LogError($"{nameof(MovementPatches)} failed: {ex}");
			}

			try // GesturePatches
			{
				GesturePatches.RegisterConfigs(Config);
				if (EnableModuleGestures.Value)
				{
					GesturePatches.Patch();
#if DEBUG
					Logger.LogInfo($"{nameof(GesturePatches)} applied");
#endif
				}
			}
			catch (System.Exception ex)
			{
				Logger.LogError($"{nameof(GesturePatches)} failed: {ex}");
			}

			try // GestureVRPatches
			{
				GestureVrPatches.RegisterConfigs(Config);
				if (EnableModuleVrGestures.Value)
				{
					GestureVrPatches.Patch();
#if DEBUG
					Logger.LogInfo($"{nameof(GestureVrPatches)} applied");
#endif
				}
			}
			catch (System.Exception ex)
			{
				Logger.LogError($"{nameof(GestureVrPatches)} failed: {ex}");
			}

			try // StatePatches
			{
				StatePatches.RegisterConfigs(Config);
				if (EnableModuleStates.Value)
				{
					StatePatches.Patch();
#if DEBUG
					Logger.LogInfo($"{nameof(StatePatches)} applied");
#endif
				}
			}
			catch (System.Exception ex)
			{
				Logger.LogError($"{nameof(StatePatches)} failed: {ex}");
			}

			try // EmotePatches
			{
				EmotePatches.RegisterConfigs(Config);
				if (EnableModuleEmotes.Value)
				{
					EmotePatches.Patch();
#if DEBUG
					Logger.LogInfo($"{nameof(EmotePatches)} applied");
#endif
				}
			}
			catch (System.Exception ex)
			{
				Logger.LogError($"{nameof(EmotePatches)} failed: {ex}");
			}
		}

		private void RegisterEnabledModuleConfigs()
		{
			EnableModuleMic = Config.Bind(
				EnabledModules,
				"EnableModuleMic",
				true,
				"If to apply mic patches. Requires a restart to take effect.");
			EnableModuleMisc = Config.Bind(
				EnabledModules,
				"EnableModuleMisc",
				true,
				"If to apply misc patches. Requires a restart to take effect.");
			EnableModuleMovement = Config.Bind(
				EnabledModules,
				"EnableModuleMovement",
				true,
				"If to apply movement patches. Requires a restart to take effect.");
			EnableModuleGestures = Config.Bind(
				EnabledModules,
				"EnableModuleGestures",
				true,
				"If to apply gesture patches. Requires a restart to take effect.");
			EnableModuleVrGestures = Config.Bind(
				EnabledModules,
				"EnableModuleVrGestures",
				true,
				"If to apply VR gesture patches. Requires a restart to take effect.");
			EnableModuleStates = Config.Bind(
				EnabledModules,
				"EnableModuleStates",
				true,
				"If to apply state patches. Requires a restart to take effect.");
			EnableModuleEmotes = Config.Bind(
				EnabledModules,
				"EnableModuleEmotes",
				true,
				"If to apply emote patches. Requires a restart to take effect.");
		}
	}
}
