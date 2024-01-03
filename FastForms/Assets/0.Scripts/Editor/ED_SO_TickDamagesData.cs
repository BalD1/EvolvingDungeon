using UnityEditor;
using StdNounou;
using static StdNounou.EditorUtils;

[CustomEditor(typeof(SO_TickDamagesData))]
public class ED_SO_TickDamagesData : Editor
{
	private SO_TickDamagesData targetScript;

    private float simulationDamages;
    private float simulationCritChances;
    private float simulationCritMultiplier;
    private float totalTriggers;

    private void OnEnable()
    {
        targetScript = (SO_TickDamagesData)target;
    }
    
    public override void OnInspectorGUI()
    {
        ReadOnlyDraws.ScriptDraw(typeof(ED_SO_TickDamagesData), targetScript);
        base.DrawDefaultInspector();

        EditorGUILayout.Space();

        if (targetScript.RequiredTicksToTrigger > 0)
            totalTriggers = targetScript.TicksLifetime / targetScript.RequiredTicksToTrigger;
        else
            totalTriggers = 0;
        EditorGUILayout.LabelField("Total time in seconds : " + targetScript.TicksLifetime * TickManager.TICK_TIMER_MAX);
        EditorGUILayout.LabelField("Total triggers : " + totalTriggers);

        SimpleDraws.HorizontalLine();

        DamagesSimulation();

        serializedObject.ApplyModifiedProperties();
    }

    private void DamagesSimulation()
    {
        EditorGUILayout.LabelField("Simulation", EditorStyles.boldLabel);
        simulationDamages = EditorGUILayout.FloatField("Damages", simulationDamages);
        simulationCritChances = EditorGUILayout.FloatField("Crit Chances", simulationCritChances);
        simulationCritMultiplier = EditorGUILayout.FloatField("Crit Multiplier", simulationCritMultiplier);

        EditorGUILayout.LabelField("Lowest Damages : " + (simulationDamages * totalTriggers));

        float averageCrits = totalTriggers * (simulationCritChances / 100);
        float averageNotCrits = totalTriggers - averageCrits;
        EditorGUILayout.LabelField("Average Damages : " + ((simulationDamages * simulationCritMultiplier * averageCrits) + (simulationDamages * averageNotCrits)));
        EditorGUILayout.LabelField("Highest Damages : " + (simulationDamages * simulationCritMultiplier * totalTriggers));
    }
}