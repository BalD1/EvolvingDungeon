using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace StdNounou
{
    public interface IStatContainer
    {
        public abstract bool TryGetStatData(E_StatType type, out StatData statData);
        public bool TryGetStatValue(E_StatType type, out float value);
        public bool TryGetHigherAllowedValue(E_StatType type, out float value);

        public SerializedDictionary<E_StatType, StatData> GetAllStats();

        public enum E_StatType
        {
            MaxHP,
            DamageReduction,
            BaseDamages,
            CritChances,
            CritMultiplier,
            AttackRange,
            AttackCooldown,
            Speed,
            Weight,
            InvincibilityCooldown,
            Piercing
        }

        [System.Serializable]
        public class StatData
        {
            [field: SerializeField] public float Value { get; private set; }
            [field: SerializeField] public float HigherAllowedValue { get; private set; }
        }
    } 
}