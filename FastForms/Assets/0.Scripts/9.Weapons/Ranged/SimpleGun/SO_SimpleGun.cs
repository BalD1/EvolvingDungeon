using StdNounou;
using UnityEngine;

[CreateAssetMenu(fileName = "New SimpleGunBehavior", menuName = "Scriptable/Weapons/Behavior")]
public class SO_SimpleGun : SO_WeaponBehavior
{
    [field: SerializeField] public Bullet BulletPrefab {  get; private set; }

    public override void Execute(Vector2 position, Quaternion rotation, Vector2 targetPosition, SO_BaseStats weaponStats, MonoStatsHandler ownerStats)
    {
        weaponStats.TryGetStatValue(IStatContainer.E_StatType.Speed, out float bulletSpeed);
        BulletPrefab.GetNext(position, rotation).Launch(targetPosition, bulletSpeed, 0, 0, 0);
    }

    public override void OnEnd()
    {
    }
    public override void OnStart()
    {
    }

    private void Awake()
    {
        Debug.Log("awake");
    }
}
