using StdNounou;
using UnityEngine;
using UnityEngine.Events;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private SO_ItemHolderBaseData itemHolderData;

    public UnityEvent OnPickup;

    private void Awake()
    {
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

        itemHolderData.AddDataToPlayer(this, player.Inventory);
        OnPickup?.Invoke();
    }

    protected virtual void OnTriggerExit2D(Collider2D collision) { }

    public SO_ItemHolderBaseData GetItemHolderData()
        => itemHolderData;
}
