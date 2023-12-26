using UnityEngine;

namespace StdNounou
{
	public static class ResourcesObjectLoader
	{
        public const string PREFABS_HOLDER_PATH = "Assets/";
		public const string PREFABS_UI_HOLDER = "UIPrefabs";
		public const string PREFABS_AUDIO_HOLDER = "AudioPrefabs";
		public const string PREFABS_WORLD_HOLDER = "WorldPrefabs";
		public const string SO_TEXTPOPUPDATA_HOLDER = "TextPopupDataHolder";

        private static SO_PrefabsHolder worldPrefabsHolder;
		private static SO_PrefabsHolder uiPrefabsHolder;
		private static SO_PrefabsHolder audioPrefabsHolder;
		private static SO_ScriptablesHolder textPopupDataHolder;
		
		public static SO_ScriptablesHolder GetTextPopupDataHolder()
			=> SetOrGetHolder(SO_TEXTPOPUPDATA_HOLDER, ref textPopupDataHolder);
        public static SO_PrefabsHolder GetWorldPrefabs()
			=> SetOrGetHolder(PREFABS_WORLD_HOLDER, ref worldPrefabsHolder);
        public static SO_PrefabsHolder GetAudioPrefabsHolder()
			=> SetOrGetHolder(PREFABS_AUDIO_HOLDER, ref uiPrefabsHolder);
        public static SO_PrefabsHolder GetUIPrefabsHolder()
			=> SetOrGetHolder(PREFABS_UI_HOLDER, ref audioPrefabsHolder);

		private static T SetOrGetHolder<T>(string holderID, ref T holder)
			where T : UnityEngine.Object
		{
			if (holder != null) return holder;
			holder = Resources.Load<T>(PREFABS_HOLDER_PATH + holderID);
			return holder;
        }
	} 
}
