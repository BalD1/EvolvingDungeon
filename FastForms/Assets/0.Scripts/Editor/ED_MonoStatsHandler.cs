using StdNounou;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static StdNounou.EditorUtils;

[CustomEditor(typeof(MonoStatsHandler))]
public class ED_MonoStatsHandler : Editor
{
	private MonoStatsHandler targetScript;

    private bool displayPermanentBonusStats;
    private bool displayTemporaryBonusStats;
    private bool displayBrutFinalStats;
    private bool displayUniqueBonusStats;
    private bool displayStackableBonusStats;

    private SO_StatModifierData modifier = null;
    private List<StatsModifier> modifiersToRemove;

    private void OnEnable()
    {
        targetScript = (MonoStatsHandler)target;
        modifiersToRemove = new List<StatsModifier>();
    }

    private void OnDisable()
    {
        modifiersToRemove = null;
    }

    public override void OnInspectorGUI()
    {
        ReadOnlyDraws.EditorScriptDraw(typeof(ED_MonoStatsHandler), this);
        base.DrawDefaultInspector();

        if (Application.isPlaying && targetScript.StatsHandler != null)
        {
            DisplayStatsDictionnary(targetScript.StatsHandler.PermanentBonusStats, "Permanent Bonuses", ref displayPermanentBonusStats);
            DisplayStatsDictionnary(targetScript.StatsHandler.TemporaryBonusStats, "Temporary Bonuses", ref displayTemporaryBonusStats);
            DisplayStatsDictionnary(targetScript.StatsHandler.BrutFinalStats, "Brut final stats", ref displayBrutFinalStats);
            DisplayUniqueStatsModifiers();
            DisplayStackableStatsModifiers();
            DisplayAddModifier();
            PerformRemoveModifiers();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayStatsDictionnary(Dictionary<IStatContainer.E_StatType, float> dictionnary, string label, ref bool foldoutValue)
    {
        using (var v1 =  new EditorGUILayout.VerticalScope("GroupBox"))
        {
            if (!(foldoutValue = EditorGUILayout.Foldout(foldoutValue, label)))
                return;
            if (dictionnary == null)
                return;
            SimpleDraws.HorizontalLine();
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            foreach (var item in dictionnary)
            {
                using (var h1 = new EditorGUILayout.HorizontalScope())
                {
                    GUI.enabled = false;
                    EditorGUILayout.LabelField(item.Key.ToString() + " : ");
                    EditorGUILayout.FloatField(item.Value);
                    GUI.enabled = true;
                }
            }
            EditorGUI.indentLevel--;
        }
    }

    private void DisplayUniqueStatsModifiers()
    {
        using (var v1 = new EditorGUILayout.VerticalScope("GroupBox"))
        {
            if (!(displayUniqueBonusStats = EditorGUILayout.Foldout(displayUniqueBonusStats, "Temporary Bonus Stats")))
                return;
            if (targetScript.StatsHandler.UniqueStatsModifiers == null)
                return;
            SimpleDraws.HorizontalLine();
            EditorGUILayout.LabelField("Unique Stats Modifiers", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            foreach (var item in targetScript.StatsHandler.UniqueStatsModifiers)
            {
                using (var h1 = new EditorGUILayout.HorizontalScope())
                {
                    GUI.enabled = false;
                    EditorGUILayout.LabelField(item.Key + " : ");
                    EditorGUILayout.ObjectField(item.Value.Data, typeof(SO_StatModifierData), false);
                    GUI.enabled = true;
                    if (GUILayout.Button("Remove"))
                        modifiersToRemove.Add(item.Value);
                }
            }
            EditorGUI.indentLevel--;
        }
    }

    private void DisplayStackableStatsModifiers()
    {
        using (var v1 = new EditorGUILayout.VerticalScope("GroupBox"))
        {
            if (!(displayStackableBonusStats = EditorGUILayout.Foldout(displayStackableBonusStats, "Stackable Bonus Stats")))
                return;
            if (targetScript.StatsHandler.StackableStatsModifiers == null)
                return;
            SimpleDraws.HorizontalLine();
            EditorGUILayout.LabelField("Stackable Stats Modifiers", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            foreach (var item in targetScript.StatsHandler.StackableStatsModifiers)
            {
                using (var v2 = new EditorGUILayout.VerticalScope("GroupBox"))
                {
                    GUI.enabled = false;
                    EditorGUILayout.LabelField(item.Key);
                    EditorGUI.indentLevel++;
                    foreach (var modifier in item.Value)
                    {
                        using (var h1 = new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.ObjectField(modifier.Data, typeof(SO_StatModifierData), false);
                            GUI.enabled = true;
                            if (GUILayout.Button("Remove"))
                                modifiersToRemove.Add(modifier);
                            GUI.enabled = false;
                        }
                    }
                    EditorGUI.indentLevel--;
                    GUI.enabled = true;
                }
            }
            EditorGUI.indentLevel--;
        }
    }

    private void DisplayAddModifier()
    {
        SimpleDraws.HorizontalLine();

        using (var h = new EditorGUILayout.HorizontalScope())
        {
            modifier = EditorGUILayout.ObjectField("Modifier Data", modifier, typeof(SO_StatModifierData), false) as SO_StatModifierData;
            if (GUILayout.Button("Add") && modifier != null)
            {
                targetScript.StatsHandler.TryAddModifier(modifier, out StatsHandler.E_ModifierAddResult result);
                this.Log("Editor Try add modifier result : " + result);
            }
        };

    }

    private void PerformRemoveModifiers()
    {
        foreach (var item in modifiersToRemove)
        {
            targetScript.StatsHandler.RemoveStatModifier(item);
        }
    }
}