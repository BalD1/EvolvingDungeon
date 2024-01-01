using UnityEngine;
using StdNounou;
using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;

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

    public Dictionary<string, ObjectPool<ParticlesPlayer>> PooledParticles { get; private set;}

    [SerializeField] private S_PoolData fireballData;
    [SerializeField] private ObjectPool<Fireball> fireballPool; 
    public ObjectPool<Fireball> FireballPool { get => fireballPool; }

    [SerializeField] private S_PoolData textPopupsData;
    [SerializeField] private ObjectPool<TextPopup> textPopupPool;
    public ObjectPool<TextPopup> TextPopupPool { get => textPopupPool; }

    protected override void EventsSubscriber()
    {
    }

    protected override void EventsUnSubscriber()
    {
    }

    protected override void Awake()
    {
        base.Awake();

        InitPools();
    }

    private void InitPools()
    {
        InitSinglePool(ref fireballPool, fireballData);
        InitSinglePool(ref  textPopupPool, textPopupsData);
    }

    private void InitSinglePool<T>(ref ObjectPool<T> pool, S_PoolData poolData)
                          where T : Component
    {
        pool = new (_createAction: () => poolData.ObjectPrefab.Create<T>(),
                    _parentContainer: poolData.Container,
                    initialCount: poolData.PoolInitialSize,
                    _maxCapacity: poolData.PoolMaxCapacity);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}