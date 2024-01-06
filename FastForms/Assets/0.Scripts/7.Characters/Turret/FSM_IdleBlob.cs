
using StdNounou;
using System;

public class FSM_IdleBlob : FSM_Base<FSM_IdleBlob.E_TurretStates>
{
    public enum E_TurretStates
    {
        Paused = -100,
        Idle = -99,
        Attacking,
    }

    public IdleBlob OwnerBlob { get; private set; }

    private MonoStatsHandler ownerStatshandler;
    public MonoStatsHandler OwnerStatsHandler { get => ownerStatshandler; }

    private EntityAnimationControllerBase ownerAnimationController;
    public EntityAnimationControllerBase OwnerAnimationController { get => ownerAnimationController; }

    private EnemiesWeaponHandler[] ownerWeaponHandlers;
    public EnemiesWeaponHandler[] OwnerWeaponHandlers { get => ownerWeaponHandlers; }

    private BaseAI ownerAI;
    public BaseAI OwnerAI { get => ownerAI; }

    private State_IdleBlob_Idle idleState;
    private State_IdleBlob_Attacking attackingState;

    public event Action OnSetup;

    protected override void OnStartedHideScreen()
    {
    }

    protected override void OnEndedShowScreen()
    {
    }

    protected override void SetupComponents()
    {
        OwnerBlob = ownerObj.GetComponent<IdleBlob>();
        ownerWeaponHandlers = new EnemiesWeaponHandler[OwnerBlob.WeaponHandlers.Length];
        for (int i = 0; i < OwnerBlob.WeaponHandlers.Length; i++)
        {
            ownerWeaponHandlers[i] = OwnerBlob.WeaponHandlers[i] as EnemiesWeaponHandler;
        }
        Owner.HolderTryGetComponent(IComponentHolder.E_Component.AI, out ownerAI);
        Owner.HolderTryGetComponent(IComponentHolder.E_Component.AnimationController, out ownerAnimationController);
        Owner.HolderTryGetComponent(IComponentHolder.E_Component.StatsHandler, out ownerStatshandler);

        this.OnSetup?.Invoke();
    }

    protected override void SetupStates()
    {
        base.SetupStates();

        idleState = new State_IdleBlob_Idle(this);
        States.Add(E_TurretStates.Idle, idleState);

        attackingState = new State_IdleBlob_Attacking(this);
        States.Add(E_TurretStates.Attacking, attackingState);
    }
}
