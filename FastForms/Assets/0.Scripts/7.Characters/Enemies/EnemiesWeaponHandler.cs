using StdNounou;
using UnityEngine;

public class EnemiesWeaponHandler : WeaponHandler
{
    [field: SerializeField] public Transform Target {  get; private set; }

    private BaseAI ownerAI;

    protected override void EventsSubscriber()
    {
    }

    protected override void EventsUnSubscriber()
    {
    }

    protected override void Awake()
    {
        base.Awake();
        if (owner.HolderTryGetComponent(IComponentHolder.E_Component.AI, out ownerAI) != IComponentHolder.E_Result.Success)
            this.LogError("Could not find AI component in owner.");
        else
            ownerAI.OnTargetChanged += SetTarget;

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
    {
        Execute(Target.position);
    }

    public void SetTarget(Transform target)
        => this.Target = target;
}
