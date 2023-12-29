using UnityEngine;

[System.Serializable]
public class State_Player_Idle : State_Player_Base
{
    public State_Player_Idle(FSM_Player ownerFSM) : base(ownerFSM)
    {
    }

    public override void EventsSubscriber()
    {
        PlayerInputsHandlerEvents.OnMovementsInputs += ReadMovementsInputs;
    }

    public override void EventsUnSubscriber()
    {
        PlayerInputsHandlerEvents.OnMovementsInputs -= ReadMovementsInputs;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void Update()
    {
        ownerFSM.PlayerRotator.SetAim();
        foreach (var item in ownerFSM.OwnerPlayer.WeaponHandlers)
        {
            item.Rotator.RotationTowardsMouse();
        }
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

    private void ReadMovementsInputs(Vector2 value)
    {
        ownerFSM.PlayerMotor.ReadMovementsInputs(value);
        ownerFSM.AskSwitchState(FSM_Player.E_PlayerStates.Moving);
    }
}
