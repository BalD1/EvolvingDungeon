using StdNounou;
using UnityEngine;

public abstract class WeaponHandler : MonoBehaviourEventsHandler
{
    [Header("Components")]
    [SerializeField] private GameObject ownerObj;
    private IComponentHolder owner;
    private MonoStatsHandler ownerStats;

    [SerializeField] protected SpriteRenderer weaponSpriteRenderer;
    [SerializeField] private Transform firePointTransform;
    [SerializeField] private Animator animator;

    [field: SerializeField] public SO_WeaponData InitialWeapon { get; private set; } 
    [field: SerializeField, ReadOnly] public SO_WeaponData CurrentWeapon { get; private set; }

    [SerializeField, ReadOnly] private float cooldown;

    protected bool isSetup;

    protected override void Awake()
    {
        base.Awake();
        owner = ownerObj.GetComponent<IComponentHolder>();
        
        owner.HolderTryGetComponent(IComponentHolder.E_Component.StatsHandler, out ownerStats);
        SetNewWeapon(InitialWeapon);
    }

    protected virtual void Update()
    {
        if (!isSetup) return;
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0) OnCooldownEnded();
        }
    }

    public void SetNewWeapon(SO_WeaponData data)
    {
        isSetup = false;
        if (data == null) return;

        CurrentWeapon?.WeaponBehavior.OnEnd();
        CurrentWeapon = data;
        CurrentWeapon.WeaponBehavior.OnStart();

        weaponSpriteRenderer.sprite = data.WeaponSprite;
        weaponSpriteRenderer.gameObject.transform.localPosition = data.SpritePivotOffset;
        animator.runtimeAnimatorController = data.WeaponAnimController;

        firePointTransform.localPosition = data.FirePointPosition;

        isSetup = true;
    }

    public void Execute()
    {
        if (CurrentWeapon == null) return;
        if (cooldown > 0) return;

        if (!CurrentWeapon.WeaponStats.TryGetStatValue(IStatContainer.E_StatType.AttackCooldown, out cooldown))
            this.LogError($"Weapon {CurrentWeapon} did not have cooldown stat.");

        animator.SetTrigger("Fire");
        CurrentWeapon.WeaponBehavior.Execute(firePointTransform.position, firePointTransform.rotation, MouseUtils.GetMouseWorldPosition(), CurrentWeapon.WeaponStats, ownerStats);
    }

    protected abstract void OnCooldownEnded();
}
