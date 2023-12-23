using StdNounou;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static StdNounou.EditorUtils;

[CustomEditor(typeof(EntityWeaponsModifierHandler))]
public class ED_EntityWeaponsModifierHandler : Editor
{
	private EntityWeaponsModifierHandler targetScript;

    private bool drawModifiers;

    private bool displayPermanentBonusStats;
    private bool displayTemporaryBonusStats;
    private bool displayBrutFinalStats;
    private bool displayUniqueBonusStats;
    private bool displayStackableBonusStats;

    private void OnEnable()
    {
        targetScript = (EntityWeaponsModifierHandler)target;
    }
    
    public override void OnInspectorGUI()
    {
        ReadOnlyDraws.EditorScriptDraw(typeof(ED_EntityWeaponsModifierHandler), this);
        base.DrawDefaultInspector();

        if (Application.isPlaying) DrawModifiers();
        
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawModifiers()
    {
        drawModifiers = EditorGUILayout.Foldout(drawModifiers, "Draw Modifiers");
        if (!drawModifiers) return;

        if (targetScript.Modifiers.GetWeaponsDatasDictionary() == null) return;
        EditorGUILayout.BeginVertical("GroupBox");
        foreach (var item in targetScript.Modifiers.GetWeaponsDatasDictionary())
        {
            EditorGUILayout.BeginVertical("GroupBox");
            EditorGUILayout.LabelField(item.Key);
            StatsHandler statsHandler = item.Value.weaponStatsModifiersHandler;
            DisplayStatsDictionnary(statsHandler.PermanentBonusStats, "Permanent Bonuses", ref displayPermanentBonusStats);
            DisplayStatsDictionnary(statsHandler.TemporaryBonusStats, "Temporary Bonuses", ref displayTemporaryBonusStats);
            DisplayStatsDictionnary(statsHandler.BrutFinalStats, "Brut final stats", ref displayBrutFinalStats);
            DisplayUniqueStatsModifiers(statsHandler);
            DisplayStackableStatsModifiers(statsHandler);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
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

    private void DisplayUniqueStatsModifiers(StatsHandler statsHandler)
    {
        EditorGUILayout.BeginVertical("GroupBox");
        if (!(displayUniqueBonusStats = EditorGUILayout.Foldout(displayUniqueBonusStats, "Temporary Bonus Stats")))
        {
            EditorGUILayout.EndVertical();
            return;
        }
        if (statsHandler.UniqueStatsModifiers == null)
        {
            EditorGUILayout.EndVertical();
            return;
        }
        SimpleDraws.HorizontalLine();
        EditorGUILayout.LabelField("Unique Stats Modifiers", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        foreach (var item in statsHandler.UniqueStatsModifiers)
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

    private void DisplayStackableStatsModifiers(StatsHandler statsHandler)
    {
        EditorGUILayout.BeginVertical("GroupBox");
        if (!(displayStackableBonusStats = EditorGUILayout.Foldout(displayStackableBonusStats, "Stackable Bonus Stats")))
        {
            EditorGUILayout.EndVertical();
            return;
        }
        if (statsHandler.StackableStatsModifiers == null)
        {
            EditorGUILayout.EndVertical();
            return;
        }
        SimpleDraws.HorizontalLine();
        EditorGUILayout.LabelField("Stackable Stats Modifiers", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        foreach (var item in statsHandler.StackableStatsModifiers)
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