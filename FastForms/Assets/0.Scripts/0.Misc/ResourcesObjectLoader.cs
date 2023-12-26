using UnityEngine;

namespace StdNounou
{
	public static class ResourcesObjectLoader
	{
        public const string PREFABS_HOLDER_PATH = "Assets/";
		public const string PREFABS_UI_HOLDER = "UIPrefabs";
		public const string PREFABS_AUDIO_HOLDER = "AudioPrefabs";
		public const string PREFABS_WORLD_HOLDER = "WorldPrefabs";

		private static SO_PrefabsHolder worldPrefabsHolder;
		private static SO_PrefabsHolder uiPrefabsHolder;
		private static SO_PrefabsHolder audioPrefabsHolder;

        public static SO_PrefabsHolder GetWorldPrefabs()
			=> SetOrGetHolder(PREFABS_WORLD_HOLDER, ref worldPrefabsHolder);
        public static SO_PrefabsHolder GetAudioPrefabsHolder()
			=> SetOrGetHolder(PREFABS_AUDIO_HOLDER, ref uiPrefabsHolder);
        public static SO_PrefabsHolder GetUIPrefabsHolder()
			=> SetOrGetHolder(PREFABS_UI_HOLDER, ref audioPrefabsHolder);
		private static SO_PrefabsHolder SetOrGetHolder(string holderID, ref SO_PrefabsHolder holder)
		{
			if (holder != null) return holder;
			holder = Resources.Load<SO_PrefabsHolder>(PREFABS_HOLDER_PATH + holderID);
			return holder;
        }
	} 
}
