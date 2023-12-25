using StdNounou;
using UnityEngine;

[CreateAssetMenu(fileName = "New SimpleGunBehavior", menuName = "Scriptable/Weapons/Behavior")]
public class SO_SimpleGun : SO_WeaponBehavior
{
    [field: SerializeField] public Bullet BulletPrefab {  get; private set; }

    public override void Execute(ref S_AttackTransform attackTransform, ref S_TotalStats totalStats)
    {
        BulletPrefab.GetNext(attackTransform.Position, attackTransform.Rotation)
                    .Launch(attackTransform.TargetPosition, ref totalStats);
    }

    public override void OnEnd()
    {
    }
    public override void OnStart()
    {
    }
}
