using UnityEngine;

public interface IProjectile<T>
{
    public T GetNext(Vector2 position, Quaternion rotation);
    public void Kill();

    public void Launch(Vector2 direction, WeaponHandler.S_WeaponData weaponData);
}