using System;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;

namespace SpoofHWID
{
	public static class BuildInfo
	{
		public const string GUID = "xyz.ljoonal.spoofhwid";

		public const string Name = "HWID Spoofer";

		public const string Version = "0.1.0";
	}

	[BepInPlugin(BuildInfo.GUID, BuildInfo.Name, BuildInfo.Version)]
	public class SpoofHWIDMod : BaseUnityPlugin
	{
		private const string ConfigCategory = "Settings";
		private static ConfigEntry<string> ConfigHWID;


		SpoofHWIDMod()
		{
			const string settingName = "HWID";
			ConfigHWID = Config.Bind(
				ConfigCategory,
				settingName,
				"",
				"The HWID to use. If it's not the correct length (by being empty for example) a new one will be generated.");

			// Following is mostly from knah's similar mod for MelonLoader, licensed under GPL3.0
			// Code & License can be found from https://github.com/knah/ML-UniversalMods
			if (ConfigHWID.Value.Length != SystemInfo.deviceUniqueIdentifier.Length)
			{
				var random = new System.Random(Environment.TickCount);
				var bytes = new byte[SystemInfo.deviceUniqueIdentifier.Length / 2];
				random.NextBytes(bytes);
				ConfigHWID.Value = string.Join("", bytes.Select(it => it.ToString("x2")));
				//ConfigHWID.Value.Save();
			}
		}

		void Awake()
		{
			try
			{
				Harmony.CreateAndPatchAll(typeof(SpoofHWIDMod));
				Logger.LogInfo("Spoofed HWID; below two should match:");
				Logger.LogInfo($"Current: {SystemInfo.deviceUniqueIdentifier}");
				Logger.LogInfo($"Target:  {ConfigHWID.Value}");
			}
			catch (Exception ex)
			{
				Logger.LogError(ex.ToString());
			}
		}

		[HarmonyPatch(typeof(SystemInfo), "deviceUniqueIdentifier")] // Specify target method with HarmonyPatch attribute
		[HarmonyPrefix]                              // There are different patch types. Prefix code runs before original code
		static bool SpoofHWID(ref string __result)
		{
			__result = ConfigHWID.Value; // The special __result variable allows you to read or change the return value
			return false; // Returning false in prefix patches skips running the original code
		}
	}
}
