using UnityEngine;

public interface IProjectile<T>
{
    public T GetNext(Vector2 position, Quaternion rotation);
    public void Kill();

    public void Launch(Vector2 direction, ref SO_WeaponBehavior.S_TotalStats totalStats, ref SO_WeaponData.S_Particles weaponParticles);
}