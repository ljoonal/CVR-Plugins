using System.Collections;
using BepInEx;
using BepInEx.Configuration;
using HopLib;
using UnityEngine;
using Friends = ABI_RC.Core.Networking.IO.Social.Friends;

namespace MoreFilters
{
	public enum ContentFilterState
	{
		Enabled = 0,
		EnabledForFriends = 1,
		Disabled = 2
	}

	[BepInDependency(HopLibInfo.Id, HopLibInfo.Version)]
	[BepInPlugin(BuildInfo.Id, BuildInfo.Name, BuildInfo.Version)]
	[BepInProcess("ChilloutVR.exe")]
	class MoreFiltersPlugin : BaseUnityPlugin
	{
		protected static bool ShouldFilter(ContentFilterState state, bool isFriend)
		{
			return state == ContentFilterState.Disabled ||
				(state == ContentFilterState.EnabledForFriends && !isFriend);
		}

		private ConfigEntry<ContentFilterState> FilterSpawnAudio;

		public void Awake()
		{
			const string FILTERS_CATEGORY = "Filters";
			FilterSpawnAudio = Config.Bind(
				FILTERS_CATEGORY,
				"SpawnAudio",
				ContentFilterState.EnabledForFriends,
				"If spawn audio should be enabled.");

			HopApi.AvatarLoaded += OnAvatarLoaded;
		}

		private void OnAvatarLoaded(object sender, AvatarEventArgs ev)
		{
			if (!ev.IsLocal) StartCoroutine(ProcessAvatarFiltering(ev));
		}

		private IEnumerator ProcessAvatarFiltering(AvatarEventArgs ev)
		{
			bool isFriend = Friends.FriendsWith(ev.Target.ownerId);

			if (!ev.Avatar.activeInHierarchy)
				yield return new WaitUntil(() => ev.Avatar == null || ev.Avatar.activeInHierarchy);

			if (ev.Avatar == null) yield break;

			if (ShouldFilter(FilterSpawnAudio.Value, isFriend))
			{
#if DEBUG
				Logger.LogInfo($"Filtering audio of {ev.Target.userName}");
#endif
				StartCoroutine(RemoveSpawnSounds(ev.Avatar));
			}
		}

		private IEnumerator RemoveSpawnSounds(GameObject gameObj)
		{
			yield return null;
			foreach (var audioSource in gameObj.GetComponentsInChildren<AudioSource>(includeInactive: true))
			{
#if DEBUG
				Logger.LogInfo($"Found audio source {audioSource.name}");
#endif
				if (audioSource != null && audioSource.isPlaying) audioSource.Stop();
			}
		}
	}
}
