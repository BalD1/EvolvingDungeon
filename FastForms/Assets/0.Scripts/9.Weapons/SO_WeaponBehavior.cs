using StdNounou;
using UnityEngine;

public abstract class SO_WeaponBehavior : ScriptableObject
{
    public abstract void OnStart();
    public abstract void OnEnd();
    public abstract void Execute(Vector2 position, Quaternion rotation, Vector2 targetPosition, WeaponHandler.S_WeaponData weaponData);
}