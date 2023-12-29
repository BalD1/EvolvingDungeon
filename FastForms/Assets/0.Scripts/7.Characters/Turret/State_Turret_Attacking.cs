
using UnityEngine;

[System.Serializable]
public class State_Turret_Attacking : State_Turret_Base
{
    private Timer delayedFireTimer;
    private float delayedFireTime = 1;

    public State_Turret_Attacking(FSM_Turret ownerFSM) : base(ownerFSM)
    {
        delayedFireTimer = new Timer(delayedFireTime, DelayedFire);
    }

    public override void EventsSubscriber()
    {
    }

    public override void EventsUnSubscriber()
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        foreach (var item in ownerFSM.OwnerWeaponHandlers)
        {
            item.SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
            item.OnCooldownEnded += OnWeaponCooldownEnded;
        }
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
        foreach (var item in ownerFSM.OwnerWeaponHandlers)
        {
            item.OnCooldownEnded -= OnWeaponCooldownEnded;
        }
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
}
