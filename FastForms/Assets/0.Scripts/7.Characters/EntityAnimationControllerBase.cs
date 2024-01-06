using StdNounou;
using System;
using UnityEngine;

public class EntityAnimationControllerBase : MonoBehaviour
{
    [SerializeField] protected GameObject ownerObj;
    private IComponentHolder owner;

    private HealthSystem ownerHealthSystem;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer rendererTarget;
    private Material rendererMaterial;

    private LTDescr flashTween = null;

    private int flashAmount = Shader.PropertyToID("_FlashAmount");

    private bool looksAtRight;

    public Animation anim;

    private const string DAMAGED_ANIM_ID = "DamagedAnim";
    private const string DAMAGED_CRIT_ANIM_ID = "DamagedCritAnim";
    private const string SQUASH_ANIM_ID = "Squash";
    private const string ATTACK_ANIM_ID = "AttackingAnim";

    public event Action AnimNotify_AttackPoint;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        rendererMaterial = rendererTarget.material;

        owner = ownerObj.GetComponent<IComponentHolder>();
        owner.HolderTryGetComponent<HealthSystem>(IComponentHolder.E_Component.HealthSystem, out ownerHealthSystem);
        ownerHealthSystem.OnDeath += OnOwnerDeath;
        ownerHealthSystem.OnTookDamages += PlayHurtAnimation;
    }

    public void TryFlip(bool toRight)
    {
        if (!(toRight ^ looksAtRight)) return;
        looksAtRight = toRight;
        rendererTarget.flipX = toRight;
    }

    private void PlayHurtAnimation(IDamageable.DamagesData damagesData)
    {
        animator.Play(damagesData.IsCrit ? 
                      DAMAGED_CRIT_ANIM_ID : 
                      DAMAGED_ANIM_ID, 0);

        animator.Play(SQUASH_ANIM_ID, 1);

        rendererMaterial.SetFloat(flashAmount, 1);
        if (flashTween != null) LeanTween.cancel(flashTween.uniqueId);
        flashTween = LeanTween.value(1, 0, .25f).setOnUpdate((float val) =>
        {
            rendererMaterial.SetFloat(flashAmount, val);
        }).setOnComplete(() => flashTween = null);
    }

    public void PlayAttackAnimation()
    {
        animator.Play(ATTACK_ANIM_ID);
    }

    public void Notify_AttackPoint()
    {
        AnimNotify_AttackPoint?.Invoke();
    }

    private void OnOwnerDeath()
    {
        ownerHealthSystem.OnDeath -= OnOwnerDeath;
        ownerHealthSystem.OnTookDamages -= PlayHurtAnimation;
    }
}