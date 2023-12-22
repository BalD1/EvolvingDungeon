using StdNounou;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EntityWeaponModifiers", menuName = "Scriptable/Weapons/EntityWeaponModifiers")]
public class SO_EntityWeaponsModifiers : ScriptableObject
{
    [SerializeField] private SO_BaseStats baseStats;

    private Dictionary<string, WeaponData> weaponDataDictionary;

    public class WeaponData
    {
        public WeaponData(SO_BaseStats baseStats)
        {
            weaponStatsModifiersHandler = new StatsHandler(baseStats);
            weaponTickDamages = new List<SO_TickDamagesData>();
        }
        public StatsHandler weaponStatsModifiersHandler;
        public List<SO_TickDamagesData> weaponTickDamages;
    }

    public void Init()
    {
        weaponDataDictionary = new Dictionary<string, WeaponData>();
    }

    public Dictionary<string, WeaponData> GetWeaponsDatasDictionary()
        => weaponDataDictionary;

    public bool TryGetWeaponData(string id, out WeaponData data)
    {
        data = null;
        if (weaponDataDictionary == null)
        {
            this.LogError("Stats Modifier Dict was null, check if Init was triggered.");
            return false;
        }
        if (!weaponDataDictionary.TryGetValue(id, out data))
        {
            weaponDataDictionary.Add(id, data = new WeaponData(baseStats));
        }
        return true;
    }

    public bool TryGetStatsModifiersHandler(string id, out StatsHandler statsHandler)
    {
        statsHandler = null;
        WeaponData weaponData = null;
        if (!TryGetWeaponData(id, out weaponData)) return false;
        statsHandler = weaponData.weaponStatsModifiersHandler;
        return true;
    }

    public bool TryGetTickDamages(string id, out List<SO_TickDamagesData> statsHandler)
    {
        statsHandler = null;
        WeaponData weaponData = null;
        if (!TryGetWeaponData(id, out weaponData)) return false;
        statsHandler = weaponData.weaponTickDamages;
        return true;
    }
}