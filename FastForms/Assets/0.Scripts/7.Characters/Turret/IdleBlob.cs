using StdNounou;
using UnityEngine;

public class IdleBlob : Entity
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

    private void Start()
    {
        if (this.HolderTryGetComponent(IComponentHolder.E_Component.AI, out BaseAI ai) == IComponentHolder.E_Result.Success)
        {
            ai.SetTarget(PlayerCharacter.Instance.transform, true);
        }
    }

    private void OnDeath()
    {
        Destroy(this.gameObject);
    }
}
