using UnityEngine;

public abstract class SO_ItemHolderBaseData<T> : ScriptableObject
    where T : ScriptableObject
{
    [field: SerializeField] public bool DestroyOnTrigger { get; private set; } = true;
    [field: SerializeField] public T Data { get; protected set; }

    public abstract void AddDataToPlayer(ItemHolder<T> sender, EntityInventory inventory);
    public abstract Sprite GetSprite();
}