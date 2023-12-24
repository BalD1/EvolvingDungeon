using StdNounou;
using UnityEngine;

public abstract class EntityMotor : ObjectMotor
{
    [SerializeField] private float steeringForce;
    [SerializeField] private float movementMass;

    private MonoStatsHandler ownerStats;

    private Vector2 steering;
    private Vector2 steeredVelocity;

    protected float steerSpeedMultiplier;

    protected override void SetComponents()
    {
        base.SetComponents();
        owner.HolderTryGetComponent(IComponentHolder.E_Component.StatsHandler, out ownerStats);
    }

    protected override void OnComponentsModifier(ComponentChangeEventArgs args)
    {
        base.OnComponentsModifier(args);
        switch (args.ComponentType)
        {
            case IComponentHolder.E_Component.StatsHandler:
                ownerStats = args.NewComponent as MonoStatsHandler;
                break;

        }
    }

    public override void MoveByVelocity()
    {
        if (stayStatic) return;
        ownerStats.StatsHandler.TryGetFinalStat(IStatContainer.E_StatType.Speed, out float speed);
        Velocity = Vector2.ClampMagnitude(Velocity, speed);

        this.Body.MovePosition(this.Body.position + Velocity * speed * Time.fixedDeltaTime);
    }

    public override void MoveTo(Vector2 goalPosition, bool steerVelocity)
    {
        Vector2 finalMovement = Vector2.zero;
        ownerStats.StatsHandler.TryGetFinalStat(IStatContainer.E_StatType.Speed, out float speed);

        if (steerVelocity)
        {
            Vector2 desiredVelocity = goalPosition * speed;

            steering = desiredVelocity - steeredVelocity;
            steering = Vector3.ClampMagnitude(steering, steeringForce);
            if (movementMass != 0)
                steering /= movementMass;

            SetSteeredVelocity(steering, speed);
            finalMovement = steeredVelocity;
        }
        else finalMovement = goalPosition * speed;

        Body.MovePosition(Body.position + finalMovement * Time.fixedDeltaTime);
    }

    private void SetSteeredVelocity(Vector2 steering, float speed)
    {
        steeredVelocity = Vector3.ClampMagnitude(steeredVelocity + steeredVelocity, speed) * steerSpeedMultiplier;
    }
}
