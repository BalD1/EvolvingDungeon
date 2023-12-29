
public class FSM_Turret : FSM_Base<FSM_Turret.E_TurretStates>
{
    public enum E_TurretStates
    {
        Idle,
        Attacking
    }

    public Turret OwnerTurret { get; private set; }

    private EnemiesWeaponHandler[] ownerWeaponHandlers;
    public EnemiesWeaponHandler[] OwnerWeaponHandlers { get => ownerWeaponHandlers; }

    private State_Turret_Idle idleState;
    private State_Turret_Attacking attackingState;

    protected override void EventsSubscriber()
    {
    }

    protected override void EventsUnSubscriber()
    {
    }

    protected override void SetupComponents()
    {
        OwnerTurret = ownerObj.GetComponent<Turret>();
        ownerWeaponHandlers = new EnemiesWeaponHandler[OwnerTurret.WeaponHandlers.Length];
        for (int i = 0; i < OwnerTurret.WeaponHandlers.Length; i++)
        {
            ownerWeaponHandlers[i] = OwnerTurret.WeaponHandlers[i] as EnemiesWeaponHandler;
        }
    }

    protected override void SetupStates()
    {
        idleState = new State_Turret_Idle(this);
        States.Add(E_TurretStates.Idle, idleState);

        attackingState = new State_Turret_Attacking(this);
        States.Add(E_TurretStates.Attacking, attackingState);
    }
}
