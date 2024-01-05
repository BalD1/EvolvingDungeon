using com.cyborgAssets.inspectorButtonPro;
using StdNounou;
using UnityEngine;

public class DoorToNextLevel : MonoBehaviourEventsHandler, IInteractable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SO_SpritesHolder doorSpritesHolder;
    [SerializeField] private RoomLoader roomLoader;
    [SerializeField] private CircleCollider2D triggerCollider;

    private Collider2D colliderInRange;

    private bool isOpen = false;
    private bool isLoading = false;

    private const string OPEN_SPRITE_ASSET = "Open";
    private const string CLOSE_SPRITE_ASSET = "Close";

    protected override void EventsSubscriber()
    {
        roomLoader.OnLoadEnded += OnLoaderLoadEnded;
    }

    protected override void EventsUnSubscriber()
    {
        roomLoader.OnLoadEnded -= OnLoaderLoadEnded;
    }

    [ProPlayButton]
    public void Open()
    {
        this.spriteRenderer.sprite = doorSpritesHolder.GetAsset(OPEN_SPRITE_ASSET);
        isOpen = true;
        triggerCollider.enabled = false;
        triggerCollider.enabled = true;
    }

    [ProPlayButton]
    public void Close()
    {
        this.spriteRenderer.sprite = doorSpritesHolder.GetAsset(CLOSE_SPRITE_ASSET);
        isOpen = false;
        RemoveFromPlayer(triggerCollider);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpen) return;
        colliderInRange = collision;
        PlayerInputsHandlerEvents.OnInteract += Interact;
        this.TryAddToPlayer(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isOpen) return;
        RemoveFromPlayer(collision);
    }

    private void RemoveFromPlayer(Collider2D collision)
    {
        if (colliderInRange != collision) return;

        colliderInRange = null;
        PlayerInputsHandlerEvents.OnInteract -= Interact;
        this.TryRemoveFromPlayer(collision);
    }

    public void Interact()
    {
        if (isLoading) return;
        RemoveFromPlayer(colliderInRange);
        isLoading = true;
        roomLoader.LoadRoom();
    }

    private void OnLoaderLoadEnded()
    {
        isLoading = false;
    }
}
