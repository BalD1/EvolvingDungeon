using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item Holder Data", menuName = "Scriptable/Items/WeaponHolderData")]
public class SO_WeaponItemHolderData : SO_ItemHolderBaseData<SO_WeaponData>
{
    public override void AddDataToPlayer(ItemHolder<SO_WeaponData> sender, EntityInventory inventory)
    {
        if (inventory.TryAddWeapon(Data) && DestroyOnTrigger)
            Destroy(sender.gameObject);
    }

    public override Sprite GetSprite()
        => Data.WeaponSprite;
}