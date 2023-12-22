using UnityEditor;
using UnityEngine;
using static StdNounou.EditorUtils;

namespace StdNounou
{
    [CustomEditor(typeof(UIScreensManager))]
    public class ED_UIScreensManager : Editor
    {
        private UIScreensManager targetScript;

        private void OnEnable()
        {
            targetScript = (UIScreensManager)target;

            UIScreenEvents.OnSubScreenStateChanged += ScreenStateChange;
        }

        private void OnDisable()
        {
            UIScreenEvents.OnSubScreenStateChanged -= ScreenStateChange;
        }

        public override void OnInspectorGUI()
        {
            ReadOnlyDraws.EditorScriptDraw(typeof(ED_UITabsHandler), this);
            base.DrawDefaultInspector();

            if (EditorApplication.isPlaying)
            {
                GUILayout.Space(5);

                SimpleDraws.HorizontalLine();
                DrawOpenScreens();
                SimpleDraws.HorizontalLine();
                DrawScreensQueue();
                Repaint();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawOpenScreens()
        {
            if (targetScript.CurrentRootScreen == null) return;
            EditorGUILayout.LabelField("Screens hierarchy");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(targetScript.CurrentRootScreen.gameObject.name);
            DrawOpenedSubs(targetScript.CurrentRootScreen);
        }

        private void DrawOpenedSubs(UIScreen screen)
        {
            if (screen == null || screen.OpenedSubscreens.Count == 0) return;
            EditorGUI.indentLevel++;
            foreach (var item in screen.OpenedSubscreens)
            {
                GUI.enabled = false;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("|->", GUILayout.MaxWidth(35 * EditorGUI.indentLevel));
                EditorGUILayout.ObjectField(item, typeof(UIScreen), true);
                EditorGUILayout.EndHorizontal();
                DrawOpenedSubs(item);
                GUI.enabled = true;
            }
            EditorGUI.indentLevel--;
        }

        private void DrawScreensQueue()
        {
            if (targetScript.ScreensStack == null) return;
            EditorGUILayout.LabelField("Screens stack");
            EditorGUILayout.Space();
            foreach (var item in targetScript.ScreensStack)
            {
                EditorGUILayout.ObjectField(item, typeof(UIScreen), true);
            }
        }

        private void ScreenStateChange(UIScreen screen)
        {
            Repaint();
        }
    } 
}