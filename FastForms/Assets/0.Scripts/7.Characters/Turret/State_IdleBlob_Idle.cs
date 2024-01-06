using StdNounou;
using UnityEngine;

[System.Serializable]
public class State_IdleBlob_Idle : State_IdleBlob_Base
{
    private float ownerAttackCooldown;

    public State_IdleBlob_Idle(FSM_IdleBlob ownerFSM) : base(ownerFSM)
    {
    }

    public override void EventsSubscriber()
    {
    }

    public override void EventsUnSubscriber()
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        ownerFSM.OwnerStatsHandler.StatsHandler.TryGetFinalStat(IStatContainer.E_StatType.AttackCooldown, out ownerAttackCooldown);
        ownerAttackCooldown = ownerAttackCooldown.Fluctuate(.2f);
    }

    public override void Update()
    {
        if (ownerAttackCooldown <= 0) CheckOwnerTarget();
        ownerAttackCooldown -= Time.deltaTime;
    }

    public override void FixedUpdate()
    {
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void Conditions()
    {
    }

    private void CheckOwnerTarget()
    {
        if (ownerFSM.OwnerAI == null) return;
        if (ownerFSM.OwnerAI.CurrentTarget == null) return;

        ownerFSM.AskSwitchState(FSM_IdleBlob.E_TurretStates.Attacking);
    }
}
