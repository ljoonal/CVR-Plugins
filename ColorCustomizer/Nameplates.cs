using UnityEngine;
using HarmonyLib;
using BepInEx.Configuration;

namespace ColorCustomizer
{
	class NameplatePatches
	{
		private static string ConfigCategory = "Nameplates";
		private static ConfigEntry<float> TalkingModifierR;
		private static ConfigEntry<float> TalkingModifierG;
		private static ConfigEntry<float> TalkingModifierB;
		private static ConfigEntry<float> TalkingModifierA;
		private static ConfigEntry<Color> NameplateColorDefault;
		private static ConfigEntry<Color> NameplateColorLegend;
		private static ConfigEntry<Color> NameplateColorCommunityGuide;
		private static ConfigEntry<Color> NameplateColorModerator;
		private static ConfigEntry<Color> NameplateColorDeveloper;

		public static void RegisterConfigs(ConfigFile Config)
		{
			TalkingModifierR = Config.Bind(
				ConfigCategory,
				"TalkingModifierR",
				0.1f,
				"How to modify the red when an user is talking. Can use negative numbers.");
			TalkingModifierG = Config.Bind(
				ConfigCategory,
				"TalkingModifierG",
				0.1f,
				"How to modify the green when an user is talking. Can use negative numbers.");
			TalkingModifierB = Config.Bind(
				ConfigCategory,
				"TalkingModifierB",
				0.1f,
				"How to modify the blue when an user is talking. Can use negative numbers.");
			TalkingModifierA = Config.Bind(
				ConfigCategory,
				"TalkingModifierA",
				0.3f,
				"How to modify the alpha when an user is talking. Can use negative numbers.");
			NameplateColorDefault = Config.Bind(
				ConfigCategory,
				"DefaultColor",
				(Color)new Color32(0, 183, 36, 50),
				"The background color for users with this rank");
			NameplateColorLegend = Config.Bind(
				ConfigCategory,
				"LegendColor",
				(Color)new Color32(50, 150, 147, 50),
				"The background color for users with this rank");
			NameplateColorCommunityGuide = Config.Bind(
				ConfigCategory,
				"CommunityGuideColor",
				(Color)new Color32(221, 90, 0, 50),
				"The background color for users with this rank");
			NameplateColorModerator = Config.Bind(
				ConfigCategory,
				"ModeratorColor",
				(Color)new Color32(221, 0, 118, 50),
				"The background color for users with this rank");
			NameplateColorDeveloper = Config.Bind(
				ConfigCategory,
				"DeveloperColor",
				(Color)new Color32(240, 0, 118, 50),
				"The background color for users with this rank");
		}

		public static void Patch()
		{
			Harmony.CreateAndPatchAll(typeof(NameplatePatches));
		}

		private static Color32 GetColorForNameplate(ABI_RC.Core.Player.PlayerNameplate nameplate)
		{
			if (nameplate.player.userRank == "Legend") return NameplateColorLegend.Value;
			if (nameplate.player.userRank == "Community Guide") return NameplateColorCommunityGuide.Value;
			if (nameplate.player.userRank == "Moderator") return NameplateColorModerator.Value;
			if (nameplate.player.userRank == "Developer") return NameplateColorDeveloper.Value;
			return NameplateColorDefault.Value;
		}

		private static Color32 ModifyColorWithTuple(Color color, (float, float, float, float) tuple)
		{
			color.r = (float)(tuple.Item1 + color.r);
			color.g = (float)(tuple.Item2 + color.g);
			color.b = (float)(tuple.Item3 + color.b);
			color.a = (float)(tuple.Item4 + color.a);
			return color;
		}

		private static void PatchNameplateColor(ABI_RC.Core.Player.PlayerNameplate instance, Color32 color)
		{
			/* If need to actually patch nameplateColor.
			typeof(ABI_RC.Core.Player.PlayerNameplate)
				.GetField("nameplateColor", BindingFlags.NonPublic | BindingFlags.Instance)
				.SetValue(instance, color);*/
			instance.nameplateBackground.color = color;
			instance.staffplateBackground.color = color;
		}

		[HarmonyPatch(typeof(ABI_RC.Core.Player.PlayerNameplate), "TalkerState")]
		[HarmonyPostfix]
		static void NormalNameplateColorPatch(float __0, ABI_RC.Core.Player.PlayerNameplate __instance)
		{
			var namePlateColor = GetColorForNameplate(__instance);
			if (__0 > 0) namePlateColor = ModifyColorWithTuple(namePlateColor,
				(TalkingModifierR.Value, TalkingModifierG.Value, TalkingModifierB.Value, TalkingModifierA.Value)
			);
			PatchNameplateColor(__instance, namePlateColor);
		}

		[HarmonyPatch(typeof(ABI_RC.Core.Player.PlayerNameplate), "UpdateNamePlate")]
		[HarmonyPostfix]
		static void TalkerNameplateColorPatch(ABI_RC.Core.Player.PlayerNameplate __instance)
		{
			PatchNameplateColor(__instance, GetColorForNameplate(__instance));
		}
	}
}
