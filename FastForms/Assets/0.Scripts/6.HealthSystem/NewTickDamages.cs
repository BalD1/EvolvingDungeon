using UnityEngine;

namespace StdNounou
{
    [System.Serializable]
    public class NewTickDamages : ITickable
    {
        [field: SerializeField, ReadOnly] public SO_TickDamagesData Data { get; private set; }
        [SerializeField, ReadOnly] private HealthSystem handler;

        private MonoStatsHandler ownerStats;
        private IDamageable.DamagesData damagesData;

        private int currentTicks;

        public NewTickDamages(SO_TickDamagesData _data, HealthSystem _handler, MonoStatsHandler stats)
        {
            this.Data = _data;

            this.handler = _handler;

            TickManagerEvents.OnTick += OnTick;

            stats = ownerStats;
            damagesData = new IDamageable.DamagesData();
            SetDamagesData();
        }

        public virtual void OnTick(int tick)
        {
            currentTicks++;
            if (currentTicks % Data.RequiredTicksToTrigger == 0)
            {
                ApplyDamages();
            }

            if (currentTicks >= Data.TicksLifetime) OnEnd();
        }

        protected virtual void ApplyDamages()
        {
            SetDamagesData();
            handler.InflictDamages(damagesData);
        }

        public void KillTick()
            => OnEnd();

        private void OnEnd()
        {
            TickManagerEvents.OnTick -= OnTick;
            handler.RemoveTickDamage(this);
        }

        public float RemainingTimeInSeconds()
            => RemainingTicks() * TickManager.TICK_TIMER_MAX;

        public int RemainingTicks()
           => Data.TicksLifetime - currentTicks;

        private void SetDamagesData()
        {
            ownerStats.TryGetFinalStat(IStatContainer.E_StatType.BaseDamages, out float ownerDamagesStat);
            bool isCrit = RandomExtensions.PercentageChance(Data.CritChances);
            damagesData.SetIsCrit(isCrit);

            if (!isCrit) damagesData.SetDamages(ownerDamagesStat * Data.Damages);
            else
            {
                float ownerCritMultiplier = -1;
                if (!ownerStats.TryGetFinalStat(IStatContainer.E_StatType.CritMultiplier, out ownerCritMultiplier))
                    ownerCritMultiplier = 1;
                damagesData.SetDamages(ownerDamagesStat * Data.Damages * ownerCritMultiplier * Data.CritMultiplier);
            }
        }
    } 
}