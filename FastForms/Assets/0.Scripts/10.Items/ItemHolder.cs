using StdNounou;
using UnityEngine;
using UnityEngine.Events;

public abstract class ItemHolder<T> : MonoBehaviourEventsHandler, IInteractable
                           where T : ScriptableObject
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private SO_ItemHolderBaseData<T> itemHolderData;

    [SerializeField] protected bool pickupOnTrigger = true;

    [SerializeField, ReadOnly] protected PlayerCharacter playerInRange;

    public UnityEvent OnPickup;

    protected override void Awake()
    {
        base.Awake();
        if (itemHolderData == null)
        {
            this.LogError($"Item Holder Data {this.gameObject.name} was null.");
            Destroy(this.gameObject);
            return;
        }
        spriteRenderer.sprite = itemHolderData.GetSprite();
        animator.SetBool("Spin", true);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCharacter player = collision.GetComponent<PlayerCharacter>();
        if (player == null) return;
        playerInRange = player;
        if (pickupOnTrigger) Interact();
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        PlayerCharacter player = collision.GetComponent<PlayerCharacter>();
        if (player == null) return;
        playerInRange = null;
    }

    public SO_ItemHolderBaseData<T> GetItemHolderData()
        => itemHolderData;

    public virtual void Interact()
    {
        if (playerInRange == null) return;
        itemHolderData.AddDataToPlayer(this, playerInRange.Inventory);
        OnPickup?.Invoke();
    }
}
