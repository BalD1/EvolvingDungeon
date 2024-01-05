
using System;

public class FSM_Turret : FSM_Base<FSM_Turret.E_TurretStates>
{
    public enum E_TurretStates
    {
        Paused = -100,
        Idle = -99,
        Attacking,
    }

    public Turret OwnerTurret { get; private set; }

    private EnemiesWeaponHandler[] ownerWeaponHandlers;
    public EnemiesWeaponHandler[] OwnerWeaponHandlers { get => ownerWeaponHandlers; }

    private BaseAI ownerAI;
    public BaseAI OwnerAI { get => ownerAI; }

    private State_Turret_Idle idleState;
    private State_Turret_Attacking attackingState;

    public event Action OnSetup;

    protected override void OnStartedHideScreen()
    {
    }

    protected override void OnEndedShowScreen()
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
        Owner.HolderTryGetComponent(IComponentHolder.E_Component.AI, out ownerAI);

        this.OnSetup?.Invoke();
    }

    protected override void SetupStates()
    {
        base.SetupStates();

        idleState = new State_Turret_Idle(this);
        States.Add(E_TurretStates.Idle, idleState);

        attackingState = new State_Turret_Attacking(this);
        States.Add(E_TurretStates.Attacking, attackingState);
    }
}
