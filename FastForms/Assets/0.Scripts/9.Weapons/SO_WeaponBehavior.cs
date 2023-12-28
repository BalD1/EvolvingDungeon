using StdNounou;
using UnityEngine;
using static SO_WeaponData;

public abstract class SO_WeaponBehavior : ScriptableObject
{
    public struct S_AttackTransform
    {
        [field: SerializeField] public Vector2 Position { get; private set; }
        [field: SerializeField] public Quaternion Rotation { get; private set; }
        [field: SerializeField] public Vector2 TargetPosition { get; private set; }

        public S_AttackTransform(Vector2 pos, Quaternion rot, Vector2 targPos)
        {
            Position = pos;
            Rotation = rot;
            TargetPosition = targPos;
        }
    }
    public struct S_TotalStats
    {
        [field: SerializeField] public SO_BaseStats WeaponStats { get; private set; }
        [field: SerializeField] public StatsHandler WeaponModifiers { get; private set; }
        [field: SerializeField] public StatsHandler OwnerStats { get; private set; }
        [field: SerializeField] public IDamageable.E_DamagesType DamageType { get; private set; }

        public S_TotalStats(SO_BaseStats weaponStats, StatsHandler weaponModifiers, StatsHandler ownerStats, IDamageable.E_DamagesType damageType)
        {
            this.WeaponStats = weaponStats;
            this.WeaponModifiers = weaponModifiers;
            this.OwnerStats = ownerStats;
            this.DamageType = damageType;
        }

        public float GetFinalStat(IStatContainer.E_StatType statType)
        {
            float weaponStat = 0;
            float ownerStatValue = 0;
            float modifierValue = 0;

            if (!WeaponStats.TryGetStatValue(statType, out weaponStat)) weaponStat = 0;
            if (!WeaponModifiers.TryGetFinalStat(statType, out modifierValue)) modifierValue = 0;
            if (!OwnerStats.TryGetFinalStat(statType, out ownerStatValue)) ownerStatValue = 0;

            return weaponStat + ownerStatValue + modifierValue;
        }
    }

    public abstract void OnStart();
    public abstract void OnEnd();
    public abstract void Execute(ref S_AttackTransform attackTransform, ref S_TotalStats totalStats, ref S_Particles particles);
}