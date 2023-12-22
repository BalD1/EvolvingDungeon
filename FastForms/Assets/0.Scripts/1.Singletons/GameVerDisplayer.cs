using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

namespace StdNounou
{
    [ExecuteInEditMode]
    public class GameVerDisplayer : MonoBehaviour
    {
        private static bool showVersion;

        private void Awake()
        {
#if UNITY_EDITOR
            SetShowVersion(true);
            if (Application.isPlaying) 
#endif
            DontDestroyOnLoad(this.gameObject);
        }

        [ProButton]
        public static void SetShowVersion(bool version)
        {
            showVersion = version;
        }

        private void OnGUI()
        {
            if (!showVersion) return;

            int lastSize = GUI.skin.label.fontSize;
            GUI.skin.label.fontSize = 30;
            Rect r = new Rect(10, Screen.height - 40, Screen.width, 250);
            GUI.Label(r, Application.version);
            GUI.skin.label.fontSize = lastSize;
        }
    }

}