using StdNounou;
using System.Collections;
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

    private void OnEnable()
    {
        targetScript = (MonoStatsHandler)target;
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
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayStatsDictionnary(Dictionary<IStatContainer.E_StatType, float> dictionnary, string label, ref bool foldoutValue)
    {
        EditorGUILayout.BeginVertical("GroupBox");
        if (!(foldoutValue = EditorGUILayout.Foldout(foldoutValue, label)))
        {
            EditorGUILayout.EndVertical();
            return;
        }
        if (dictionnary == null)
        {
            EditorGUILayout.EndVertical();
            return;
        }
        SimpleDraws.HorizontalLine();
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        foreach (var item in dictionnary)
        {
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            EditorGUILayout.LabelField(item.Key.ToString() + " : ");
            EditorGUILayout.FloatField(item.Value);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }

    private void DisplayUniqueStatsModifiers()
    {
        EditorGUILayout.BeginVertical("GroupBox");
        if (!(displayUniqueBonusStats = EditorGUILayout.Foldout(displayUniqueBonusStats, "Temporary Bonus Stats")))
        {
            EditorGUILayout.EndVertical();
            return;
        }
        if (targetScript.StatsHandler.UniqueStatsModifiers == null)
        {
            EditorGUILayout.EndVertical();
            return;
        }
        SimpleDraws.HorizontalLine();
        EditorGUILayout.LabelField("Unique Stats Modifiers", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        foreach (var item in targetScript.StatsHandler.UniqueStatsModifiers)
        {
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            EditorGUILayout.LabelField(item.Key + " : ");
            EditorGUILayout.ObjectField(item.Value.Data, typeof(SO_StatModifierData), false);
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }

    private void DisplayStackableStatsModifiers()
    {
        EditorGUILayout.BeginVertical("GroupBox");
        if (!(displayStackableBonusStats = EditorGUILayout.Foldout(displayStackableBonusStats, "Stackable Bonus Stats")))
        {
            EditorGUILayout.EndVertical();
            return;
        }
        if (targetScript.StatsHandler.StackableStatsModifiers == null)
        {
            EditorGUILayout.EndVertical();
            return;
        }
        SimpleDraws.HorizontalLine();
        EditorGUILayout.LabelField("Stackable Stats Modifiers", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        foreach (var item in targetScript.StatsHandler.StackableStatsModifiers)
        {
            EditorGUILayout.BeginVertical("GroupBox");
            GUI.enabled = false;
            EditorGUILayout.LabelField(item.Key);
            EditorGUI.indentLevel++;
            foreach (var modifier in item.Value)
            {
                EditorGUILayout.ObjectField(modifier.Data, typeof(SO_StatModifierData), false);
            }
            EditorGUI.indentLevel--;
            GUI.enabled = true;
            EditorGUILayout.EndVertical();
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
    }
}