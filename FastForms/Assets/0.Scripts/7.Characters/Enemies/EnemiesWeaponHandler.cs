using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesWeaponHandler : WeaponHandler
{
    [field:SerializeField] public Transform Target {  get; private set; }

    protected override void EventsSubscriber()
    {
    }

    protected override void EventsUnSubscriber()
    {
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void AimTarget()
    {
        Rotator.RotationTowardsPosition(Target.position);
    }

    public void ExecuteAtTarget()
        => Execute(Target.position);

    public void SetTarget(Transform target)
        => this.Target = target;
}
