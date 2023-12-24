using StdNounou;
using UnityEngine;

[CreateAssetMenu(fileName = "New SimpleGunBehavior", menuName = "Scriptable/Weapons/Behavior")]
public class SO_SimpleGun : SO_WeaponBehavior
{
    [field: SerializeField] public Bullet BulletPrefab {  get; private set; }

    public override void Execute(Vector2 position, Quaternion rotation, Vector2 targetPosition, WeaponHandler.S_WeaponData weaponData)
    {
        BulletPrefab.GetNext(position, rotation).Launch(targetPosition, weaponData);
    }

    public override void OnEnd()
    {
    }
    public override void OnStart()
    {
    }
}
