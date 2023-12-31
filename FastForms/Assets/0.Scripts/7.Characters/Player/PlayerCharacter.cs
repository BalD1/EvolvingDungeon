

using StdNounou;

public class PlayerCharacter : Entity
{
    protected override void Awake()
    {
        base.Awake();
        this.PlayerCreated();

        if (this.HolderTryGetComponent(IComponentHolder.E_Component.HealthSystem, out HealthSystem healthSystem) == IComponentHolder.E_Result.Success)
        {
            healthSystem.OnDeath += OnDeath;
        }
    }

    private void OnDeath()
    {
        this.PlayerDeath();
        Destroy(this.gameObject);
    }
}
