using StdNounou;
using UnityEngine;

public class DoorToNextLevel : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SO_SpritesHolder doorSpritesHolder;

    private const string OPEN_SPRITE_ASSET = "Open";
    private const string CLOSE_SPRITE_ASSET = "Close";

    public void Open()
    {
        this.spriteRenderer.sprite = doorSpritesHolder.GetAsset(OPEN_SPRITE_ASSET);
    }

    public void Close()
    {
        this.spriteRenderer.sprite = doorSpritesHolder.GetAsset(CLOSE_SPRITE_ASSET);
    }
}
