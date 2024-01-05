using StdNounou;
using UnityEngine;
using UnityEngine.InputSystem;

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
        ownerFSM.PlayerMotor.ForceReadMovementsInput();
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
    }

    private void ReadMovementsInputs(InputAction.CallbackContext context)
    {
        ownerFSM.PlayerMotor.ReadMovementsInputs(context);
        ownerFSM.AskSwitchState(FSM_Player.E_PlayerStates.Moving);
    }
}
