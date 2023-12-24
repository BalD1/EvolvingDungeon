using StdNounou;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private SO_ItemHolderBaseData itemHolderData;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCharacter player = collision.GetComponent<PlayerCharacter>();
        if (player == null) return;

        itemHolderData.AddDataToPlayer(this, player.Inventory);
    }
}
