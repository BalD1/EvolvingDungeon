
using UnityEngine;

public class FSM_Player : FSM_Base<FSM_Player.E_PlayerStates>
{
    public enum E_PlayerStates
    {
        Paused = -100,
        Idle = -99,
        Moving,
    }

    public PlayerCharacter OwnerPlayer {  get; private set; }

    private PlayerMotor playerMotor;
    public PlayerMotor PlayerMotor { get => playerMotor; }

    private PlayerAnimationController ownerAnimationController;
    public PlayerAnimationController OwnerAnimationController { get => ownerAnimationController; }

    private State_Player_Idle idleState;
    private State_Player_Moving movingState;

    protected override void EventsSubscriber()
    {
        base.EventsSubscriber();
    }

    protected override void EventsUnSubscriber()
    {
        base.EventsUnSubscriber();
    }

    protected override void OnStartedHideScreen()
    {
        AskSwitchState(E_PlayerStates.Paused);
    }

    protected override void OnEndedShowScreen()
    {
        AskSwitchState(E_PlayerStates.Idle);
    }

    protected override void SetupComponents()
    {
        OwnerPlayer = ownerObj.GetComponent<PlayerCharacter>();
        Owner.HolderTryGetComponent(IComponentHolder.E_Component.Motor, out playerMotor);
        Owner.HolderTryGetComponent(IComponentHolder.E_Component.AnimationController, out ownerAnimationController);
    }

    protected override void SetupStates()
    {
        base.SetupStates();

        idleState = new State_Player_Idle(this);
        States.Add(E_PlayerStates.Idle, idleState);

        movingState = new State_Player_Moving(this);
        States.Add(E_PlayerStates.Moving, movingState);
    }

    public override void ResetMotor()
    {
        PlayerMotor.SetAllVelocity(Vector2.zero);
    }
}