using StdNounou;
using UnityEngine;

[CreateAssetMenu(fileName = "New EntityWeaponModifiers", menuName = "Scriptable/Weapons/EntityModifiers")]
public class SO_EntityWeaponsModifiers : ScriptableObject
{
    private MonoStatsHandler statsHandler;

    public MonoStatsHandler GetStatsHandler { get => statsHandler; }
}