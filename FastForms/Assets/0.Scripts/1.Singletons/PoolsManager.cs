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

    [SerializeField] private S_PoolData bulletsData;
    [SerializeField] private ObjectPool<Bullet> bulletsPool; 
    public ObjectPool<Bullet> BulletsPool { get => bulletsPool; }

    [SerializeField] private S_PoolData textPopupsData;
    [SerializeField] private ObjectPool<TextPopup> textPopupPool;
    public ObjectPool<TextPopup> TextPopupPool { get => textPopupPool; }

    [SerializeField] private SerializedDictionary<PooledParticlesPlayer.E_ParticlesPoolID, S_PoolData> particlesData;
    public Dictionary<PooledParticlesPlayer.E_ParticlesPoolID, ObjectPool<PooledParticlesPlayer>> ParticlesDictionary { get; private set; }

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
        InitSinglePool(ref bulletsPool, bulletsData);
        InitSinglePool(ref  textPopupPool, textPopupsData);

        ParticlesDictionary = new Dictionary<PooledParticlesPlayer.E_ParticlesPoolID, ObjectPool<PooledParticlesPlayer>>();
        foreach (PooledParticlesPlayer.E_ParticlesPoolID val in Enum.GetValues(typeof(PooledParticlesPlayer.E_ParticlesPoolID)))
        {
            S_PoolData data = particlesData[val];
            ObjectPool<PooledParticlesPlayer> particlesPool = new ObjectPool<PooledParticlesPlayer>(
                _createAction: () => data.ObjectPrefab.Create<PooledParticlesPlayer>(),
                _parentContainer: data.Container,
                initialCount: data.PoolInitialSize,
                _maxCapacity: data.PoolMaxCapacity
                );
            ParticlesDictionary.Add(val, particlesPool);
        }
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