using UnityEngine;

public class AnimationControllerBase : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer rendererTarget;
    private bool looksAtRight;

    public void TryFlip(bool toRight)
    {
        if (!(toRight ^ looksAtRight)) return;
        rendererTarget.flipX = toRight;
    }

}
