
using System;

namespace StdNounou
{
    public interface IDamageable
    {
        public abstract void UpdateMaxHealth(float newMaxHealth, bool healDifference);
        public abstract bool TryInflictDamages(DamagesData damagesData);
        public abstract void InflictDamages(DamagesData damagesData);
        public abstract void Heal(float amount, bool isCrit);
        public abstract void Kill();
        public bool IsAlive();

        public class DamagesData : EventArgs
        {
            public SO_BaseStats.E_Team DamagerTeam { get; private set; }
            public E_DamagesType DamagesType { get; private set; }
            public bool IsCrit { get; private set; }
            public float Damages { get; private set; }

            public DamagesData()
            {
                this.DamagerTeam = SO_BaseStats.E_Team.Neutral;
                this.DamagesType = E_DamagesType.Unknown;
                this.Damages = -1;
            }
            public DamagesData(SO_BaseStats.E_Team damagerTeam, E_DamagesType damagesType, float damages, bool isCrit)
            {
                this.DamagerTeam = damagerTeam;
                this.DamagesType = damagesType;
                this.Damages = damages;
                this.IsCrit = isCrit;
            }
            public DamagesData(WeaponHandler.S_WeaponData weaponData)
            {
                this.DamagerTeam = weaponData.team;
                this.DamagesType = weaponData.damagesType;
                this.Damages = weaponData.damages;

                this.IsCrit = RandomExtensions.PercentageChance(weaponData.critChances);
                if (IsCrit) this.Damages *= weaponData.critMultiplier;
            }

            public void SetIsCrit(bool isCrit)
                => IsCrit = isCrit;

            public void SetDamages(float damages)
                => Damages = damages;
        }

        public enum E_DamagesType
        {
            Unknown,
            Blunt,
            Sharp,
            Flames,
        }
    } 
}
