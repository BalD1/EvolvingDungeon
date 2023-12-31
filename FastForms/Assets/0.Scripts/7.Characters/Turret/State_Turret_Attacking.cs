
using UnityEngine;

[System.Serializable]
public class State_Turret_Attacking : State_Turret_Base
{
    private Timer delayedFireTimer;

    public State_Turret_Attacking(FSM_Turret ownerFSM) : base(ownerFSM)
    {
        ownerFSM.OnSetup += OnFSMSetup;
    }

    public override void Dispose()
    {
        base.Dispose();
        PlayerCharacterEvents.OnPlayerDeath -= OnPlayerDeath;
        if (ownerFSM.OwnerAI != null)
            ownerFSM.OwnerAI.OnTargetChanged -= OnTargetChanged;
    }

    private void OnFSMSetup()
    {
        delayedFireTimer = new Timer(ownerFSM.OwnerTurret.TimeBeforeStartAttacking, DelayedFire);

        ownerFSM.OnSetup -= OnFSMSetup;
    }

    public override void EventsSubscriber()
    {
        PlayerCharacterEvents.OnPlayerDeath += OnPlayerDeath;
        ownerFSM.OwnerAI.OnTargetChanged += OnTargetChanged;

        foreach (var item in ownerFSM.OwnerWeaponHandlers)
            item.OnCooldownEnded += OnWeaponCooldownEnded;
    }

    public override void EventsUnSubscriber()
    {
        PlayerCharacterEvents.OnPlayerDeath -= OnPlayerDeath;
        ownerFSM.OwnerAI.OnTargetChanged -= OnTargetChanged;

        foreach (var item in ownerFSM.OwnerWeaponHandlers)
            item.OnCooldownEnded -= OnWeaponCooldownEnded;
    }

    public override void EnterState()
    {
        base.EnterState();
        delayedFireTimer.Reset();
    }

    public override void Update()
    {
        foreach (var item in ownerFSM.OwnerWeaponHandlers)
        {
            item.AimTarget();
        }
    }

    public override void FixedUpdate()
    {
    }

    public override void ExitState()
    {
        delayedFireTimer.Stop();
        base.ExitState();
    }

    public override void Conditions()
    {
    }

    private void DelayedFire()
    {
        foreach (var item in ownerFSM.OwnerWeaponHandlers)
        {
            item.ExecuteAtTarget();
        }
    }

    private void OnWeaponCooldownEnded(WeaponHandler weaponHandler)
    {
        (weaponHandler as EnemiesWeaponHandler).ExecuteAtTarget();
    }

    private void OnTargetChanged(Transform newTarget)
    {
        if (newTarget == null) this.ownerFSM.AskSwitchState(FSM_Turret.E_TurretStates.Idle);
    }
    private void OnPlayerDeath(PlayerCharacter playerCharacter)
    {
        this.ownerFSM.AskSwitchState(FSM_Turret.E_TurretStates.Idle);
    }
}
