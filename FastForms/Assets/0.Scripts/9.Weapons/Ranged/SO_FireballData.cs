using StdNounou;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet Data", menuName = "Scriptable/Weapons/Bullets/Data")]
public class SO_FireballData : ScriptableObject
{
    [field: SerializeField] public float Lifetime { get; private set; } = 2;
    [field: SerializeField] public float CollisionCheckCooldown { get; private set; } = .05f;
    [field: SerializeField] public float OverlapRadius { get; private set; } = 0.125f;
    [field: SerializeField] public LayerMask TargetMask { get; private set; }
    [field: SerializeField] public SO_TickDamagesData TickDamagesData { get; private set; }
}