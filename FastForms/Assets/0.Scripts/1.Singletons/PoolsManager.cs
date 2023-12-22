using UnityEngine;
using StdNounou;
using AdvancedEditorTools.DataTypes;
using System;

public class PoolsManager : Singleton<PoolsManager>
{
    [System.Serializable]
    public struct S_PoolData
    {
        [field: SerializeField] public int PoolInitialSize { get; private set; }
        [field: SerializeField] public int PoolMaxCapacity { get; private set; }
        [field: SerializeField] public GameObject ObjectPrefab { get; private set; }
        [field: SerializeField] public Transform Container {  get; private set; }
    }
    [field: SerializeField] public S_PoolData BulletsData { get; private set; }
    [field: SerializeField] public PoolableObject<Bullet> BulletsPool { get; private set; }

    protected override void EventsSubscriber()
    {
    }

    protected override void EventsUnSubscriber()
    {
    }

    protected override void Awake()
    {
        base.Awake();

        BulletsPool = new (_createAction: () => BulletsData.ObjectPrefab.Create<Bullet>(),
                           _parentContainer: BulletsData.Container,
                           initialCount: BulletsData.PoolInitialSize);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}