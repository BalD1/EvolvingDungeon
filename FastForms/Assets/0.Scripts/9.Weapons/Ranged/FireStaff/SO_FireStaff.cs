using StdNounou;
using UnityEngine;
using static SO_WeaponData;

[CreateAssetMenu(fileName = "New FireStaffBehavior", menuName = "Scriptable/Weapons/Behavior")]
public class SO_FireStaff : SO_WeaponBehavior
{
    public override void Execute(ref S_AttackTransform attackTransform, ref S_TotalStats totalStats, ref S_Particles weaponParticles)
    {
        BulletPrefab.GetNext(attackTransform.Position, attackTransform.Rotation)
                    .Launch(attackTransform.TargetPosition, ref totalStats, ref weaponParticles);
    }

    public override void OnEnd()
    {
    }
    public override void OnStart()
    {
    }
}
