
public class FSM_Player : FSM_Base<FSM_Player.E_PlayerStates>
{
    public enum E_PlayerStates
    {
        Idle,
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
    }

    protected override void EventsUnSubscriber()
    {
    }

    protected override void SetupComponents()
    {
        OwnerPlayer = ownerObj.GetComponent<PlayerCharacter>();
        Owner.HolderTryGetComponent<PlayerMotor>(IComponentHolder.E_Component.Motor, out playerMotor);
        Owner.HolderTryGetComponent(IComponentHolder.E_Component.AnimationController, out ownerAnimationController);
    }

    protected override void SetupStates()
    {
        idleState = new State_Player_Idle(this);
        States.Add(E_PlayerStates.Idle, idleState);

        movingState = new State_Player_Moving(this);
        States.Add(E_PlayerStates.Moving, movingState);
    }
}