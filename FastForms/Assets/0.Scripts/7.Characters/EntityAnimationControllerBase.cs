using StdNounou;
using UnityEngine;

public class EntityAnimationControllerBase : MonoBehaviour
{
    [SerializeField] private GameObject ownerObj;
    private IComponentHolder owner;

    private HealthSystem ownerHealthSystem;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer rendererTarget;
    private bool looksAtRight;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        owner = ownerObj.GetComponent<IComponentHolder>();
        owner.HolderTryGetComponent<HealthSystem>(IComponentHolder.E_Component.HealthSystem, out ownerHealthSystem);
        ownerHealthSystem.OnTookDamages += PlayHurtAnimation;
    }

    public void TryFlip(bool toRight)
    {
        if (!(toRight ^ looksAtRight)) return;
        rendererTarget.flipX = toRight;
    }

    private void PlayHurtAnimation(IDamageable.DamagesData damagesData)
    {
        animator.Play("DamagedAnim", 0);
    }
}