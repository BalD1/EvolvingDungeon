using StdNounou;
using UnityEngine;

public class Turret : Entity
{
    [field: SerializeField] public float TimeBeforeStartAttacking { get; private set; } = 1;

    protected override void Awake()
    {
        base.Awake();

        if (this.HolderTryGetComponent(IComponentHolder.E_Component.HealthSystem, out HealthSystem healthSystem) == IComponentHolder.E_Result.Success)
        {
            healthSystem.OnDeath += OnDeath;
        }
    }

    private void OnDeath()
    {
        Destroy(this.gameObject);
    }
}
