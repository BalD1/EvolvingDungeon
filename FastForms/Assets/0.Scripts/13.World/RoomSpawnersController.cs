using StdNounou;
using System;
using UnityEngine;

public class RoomSpawnersController : MonoBehaviourEventsHandler, IRoomCondition
{
    [SerializeField] private Room room;
    [SerializeField] private EntitySpawner[] entitySpawners;

    public event Action<IRoomCondition> OnAllEntitiesDied;

    [SerializeField, ReadOnly] private int killableEntitiesCount = 0;

    private void Reset()
    {
        room = this.GetComponentInParent<Room>();
    }

    protected override void EventsSubscriber()
    {
    }

    protected override void EventsUnSubscriber()
    {
    }

    private void OnRoomLoaded(Room room)
    {

    }

    private void OnSpawnedEntityDeath()
    {
        killableEntitiesCount--;
        if (killableEntitiesCount <= 0) OnAllEntitiesDied?.Invoke(this);
    }

    public void Setup()
    {
        foreach (var item in entitySpawners)
        {
            Entity result = item.PerformSpawn();
            if (result.HolderTryGetComponent(IComponentHolder.E_Component.HealthSystem, out HealthSystem resultHealthSystem)
                       != IComponentHolder.E_Result.Success) continue;

            resultHealthSystem.OnDeath += OnSpawnedEntityDeath;
            killableEntitiesCount++;
        }

        if (killableEntitiesCount <= 0) OnAllEntitiesDied?.Invoke(this);
    }

    public void AddListener(Action<IRoomCondition> listener)
    {
        OnAllEntitiesDied += listener;
    }

    public void RemoveListener(Action<IRoomCondition> listener)
    {
        OnAllEntitiesDied -= listener;
    }
}
