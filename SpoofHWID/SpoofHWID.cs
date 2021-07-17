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


			if (ConfigHWID.Value.Length != SystemInfo.deviceUniqueIdentifier.Length)
			{
				Logger.LogInfo("Generating new HWID");
				ConfigHWID.Value = RandomHWID();
			}
		}

		// Following is mostly from knah's similar mod for MelonLoader, licensed under GPL3.0
		// Code & License can be found from https://github.com/knah/ML-UniversalMods
		private string RandomHWID()
		{
			var random = new System.Random(Environment.TickCount);
			var bytes = new byte[SystemInfo.deviceUniqueIdentifier.Length / 2];
			random.NextBytes(bytes);
			return string.Join("", bytes.Select(it => it.ToString("x2")));
		}

		void Awake()
		{
			Logger.LogInfo($"HWID before patch: {SystemInfo.deviceUniqueIdentifier}");
			Harmony.CreateAndPatchAll(typeof(SpoofHWIDMod));
			Logger.LogInfo($"HWID after patch: {SystemInfo.deviceUniqueIdentifier}");
			Logger.LogInfo($"HWID target:  {ConfigHWID.Value}");
		}

		[HarmonyPatch(typeof(SystemInfo), "deviceUniqueIdentifier", MethodType.Getter)]
		[HarmonyPrefix]
		static bool SpoofHWID(ref string __result)
		{
			__result = ConfigHWID.Value;
			return false; // Skip running the original code
		}
	}
}
