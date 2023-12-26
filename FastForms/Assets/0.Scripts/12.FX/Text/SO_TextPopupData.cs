using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TextPopupData", menuName = "Scriptable/FX/TextData")]
public class SO_TextPopupData : ScriptableObject
{
    [field: SerializeField] public float TravelSpeed { get; private set; } = 5;
    [field: SerializeField] public float FadeSpeed { get; private set; } = 5;
    [field: SerializeField] public float Lifetime { get; private set; } = 1;
    [field: SerializeField] public Vector3 TargetPosition { get; private set; }

    [field: SerializeField, Range(0, 100)] public float AlphaFadeLifetimeStart { get; private set; } = 50;
}