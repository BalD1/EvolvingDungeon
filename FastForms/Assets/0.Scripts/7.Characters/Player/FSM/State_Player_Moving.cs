using StdNounou;
using UnityEngine;

[System.Serializable]
public class State_Player_Moving : State_Player_Base
{
    public State_Player_Moving(FSM_Player ownerFSM) : base(ownerFSM)
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
        foreach (var item in ownerFSM.OwnerPlayer.WeaponHandlers)
        {
            item.Rotator.RotationTowardsMouse();
        }
        ownerFSM.OwnerAnimationController.TryFlip(MouseUtils.GetMouseWorldPosition().x < ownerFSM.OwnerPlayer.transform.position.x);
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
        if (ownerFSM.PlayerMotor.IsIdle())
            ownerFSM.AskSwitchState(FSM_Player.E_PlayerStates.Idle);
    }

    private void ReadMovementsInputs(Vector2 value)
    {
        ownerFSM.PlayerMotor.ReadMovementsInputs(value);
    }
}
