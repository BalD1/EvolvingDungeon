using UnityEngine;

[System.Serializable]
public class State_Turret_Idle : State_Turret_Base
{
    public State_Turret_Idle(FSM_Turret ownerFSM) : base(ownerFSM)
    {
    }

    public override void EventsSubscriber()
    {
        ownerFSM.OwnerAI.OnTargetChanged += OnTargetChanged;
    }

    public override void EventsUnSubscriber()
    {
        ownerFSM.OwnerAI.OnTargetChanged -= OnTargetChanged;
    }

    public override void EnterState()
    {
        base.EnterState();
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

    private void OnTargetChanged(Transform newTarget)
    {
        if (newTarget != null) this.ownerFSM.AskSwitchState(FSM_Turret.E_TurretStates.Attacking);
    }
}
