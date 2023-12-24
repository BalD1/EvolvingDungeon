using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item Holder Data", menuName = "Scriptable/Items/WeaponHolderData")]
public class SO_WeaponItemHolderData : SO_ItemHolderBaseData
{
    [SerializeField] private SO_WeaponData weaponData;

    public override void AddDataToPlayer(ItemHolder sender, EntityInventory inventory)
    {
        if (inventory.TryAddWeapon(weaponData) && DestroyOnTrigger)
            Destroy(sender.gameObject);
    }

    public override Sprite GetSprite()
        => weaponData.WeaponSprite;
}