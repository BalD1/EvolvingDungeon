using UnityEngine;

public abstract class SO_ItemHolderBaseData : ScriptableObject
{
    public bool DestroyOnTrigger { get; private set; } = true;

    public abstract void AddDataToPlayer(ItemHolder sender, EntityInventory inventory);
    public abstract Sprite GetSprite();
}