

using StdNounou;

public class PlayerCharacter : Entity
{
    public static PlayerCharacter Instance { get; private set; }

    protected override void EventsSubscriber()
    {
        base.EventsSubscriber();
        RoomEvents.OnRoomLoaded += OnRoomLoaded;
    }

    protected override void EventsUnSubscriber()
    {
        base.EventsUnSubscriber();
        RoomEvents.OnRoomLoaded -= OnRoomLoaded;
    }

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
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

    private void OnRoomLoaded(Room room)
    {
        this.Teleport(room.PlayerSpawnPoint.position);
    }
}
