using UnityEngine;

public class EntityInventory
{
    private Entity owner;
    
    public EntityInventory(Entity owner)
    { this.owner = owner; }

    public bool TryAddWeapon(SO_WeaponData weaponData)
    {
        foreach (var item in owner.WeaponHandlers)
        {
            if (item.CurrentWeapon != null) continue;
            item.SetNewWeapon(weaponData);
            return true;
        }
        return false;
    }
}
