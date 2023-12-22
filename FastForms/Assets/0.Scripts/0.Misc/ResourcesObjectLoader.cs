using UnityEngine;

namespace StdNounou
{
	public static class ResourcesObjectLoader
	{
        public const string PREFABS_HOLDER_PATH = "Assets/";
		public const string PREFABS_UI_HOLDER = "UIPrefabs";
		public const string PREFABS_AUDIO_HOLDER = "AudioPrefabs";

        public static SO_PrefabsHolder GetAudioPrefabsHolder()
			=> GetPrefabsHolder(PREFABS_AUDIO_HOLDER);
        public static SO_PrefabsHolder GetUIPrefabsHolder()
			=> GetPrefabsHolder(PREFABS_UI_HOLDER);
        public static SO_PrefabsHolder GetPrefabsHolder(string holderID)
		{
			return Resources.Load<SO_PrefabsHolder>(PREFABS_HOLDER_PATH + holderID);
		}
	} 
}
