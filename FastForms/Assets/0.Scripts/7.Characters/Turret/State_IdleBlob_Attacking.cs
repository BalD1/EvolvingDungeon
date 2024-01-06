
using UnityEngine;

[System.Serializable]
public class State_IdleBlob_Attacking : State_IdleBlob_Base
{
    public State_IdleBlob_Attacking(FSM_IdleBlob ownerFSM) : base(ownerFSM)
    {
        ownerFSM.OnSetup += OnFSMSetup;
    }

    public override void Dispose()
    {
        base.Dispose();
        if (ownerFSM.OwnerAI != null)
            ownerFSM.OwnerAI.OnTargetChanged -= OnTargetChanged;

        if (ownerFSM.OwnerAnimationController != null)
            ownerFSM.OwnerAnimationController.AnimNotify_AttackPoint -= PerformAttack;
    }

    private void OnFSMSetup()
    {
        ownerFSM.OnSetup -= OnFSMSetup;
    }

    public override void EventsSubscriber()
    {
        ownerFSM.OwnerAI.OnTargetChanged += OnTargetChanged;
        ownerFSM.OwnerAnimationController.AnimNotify_AttackPoint += PerformAttack;
    }

    public override void EventsUnSubscriber()
    {
        ownerFSM.OwnerAI.OnTargetChanged -= OnTargetChanged;
        ownerFSM.OwnerAnimationController.AnimNotify_AttackPoint -= PerformAttack;

    }

    public override void EnterState()
    {
        base.EnterState();

        if (ownerFSM.OwnerAnimationController == null)
            PerformAttack();
        else ownerFSM.OwnerAnimationController.PlayAttackAnimation();
    }

    public override void Update()
    {
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

    private void PerformAttack()
    {
        foreach (var item in ownerFSM.OwnerWeaponHandlers)
        {
            item.AimTarget();
            item.ExecuteAtTarget();
        }
        ownerFSM.AskSwitchState(FSM_IdleBlob.E_TurretStates.Idle);
    }

    private void OnTargetChanged(Transform newTarget)
    {
        if (newTarget == null) this.ownerFSM.AskSwitchState(FSM_IdleBlob.E_TurretStates.Idle);
    }
}
