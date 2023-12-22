using UnityEngine;

namespace StdNounou
{
    [CreateAssetMenu(fileName = "New TickDamages Data", menuName = "Scriptable/TickDamages/Data")]
    public class SO_TickDamagesData : ScriptableObject
    {
        [field: SerializeField] public string ID { get; private set; }

        [field: SerializeField] public IDamageable.E_DamagesType DamagesType { get; private set; }

        [field: SerializeField] public bool Stackable { get; private set; }

        [field: SerializeField] public int TicksLifetime { get; private set; }
        [field: SerializeField] public int RequiredTicksToTrigger { get; private set; }

        [field: SerializeField] public float Damages { get; private set; }
        [field: SerializeField] public int CritChances { get; private set; }
        [field: SerializeField] public float CritMultiplier { get; private set; }
    } 
}