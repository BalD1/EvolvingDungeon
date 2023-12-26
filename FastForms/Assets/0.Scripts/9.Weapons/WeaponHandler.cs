using com.cyborgAssets.inspectorButtonPro;
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
    [field: SerializeField, ReadOnly] public SO_EntityWeaponsModifiers OwnerWeaponModifiers { get; private set; }

    [SerializeField, ReadOnly] private float cooldown;

    [field: SerializeField, ReadOnly] public bool AllowExecution { get; private set; } = true;

    protected bool isSetup;

    protected override void Awake()
    {
        base.Awake();

        owner = ownerObj.GetComponent<IComponentHolder>();
        
        owner.HolderTryGetComponent(IComponentHolder.E_Component.StatsHandler, out ownerStats);
        owner.HolderTryGetComponent(IComponentHolder.E_Component.WeaponStatsModifierHandler, out EntityWeaponsModifierHandler handler);
        OwnerWeaponModifiers = handler.Modifiers;

        if (InitialWeapon != null) SetNewWeapon(InitialWeapon);
        else if (CurrentWeapon != null) SetNewWeapon(CurrentWeapon);
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

    [ProPlayButton]
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


        animator.speed = 1/cooldown;
        animator.SetTrigger("Fire");

        SO_WeaponBehavior.S_AttackTransform attackTransform = new SO_WeaponBehavior.S_AttackTransform(
            pos: firePointTransform.position,
            rot: firePointTransform.rotation,
            targPos: MouseUtils.GetMouseWorldPosition()
            );

        StatsHandler weaponModifiers = null;
        this.OwnerWeaponModifiers.TryGetStatsModifiersHandler(this.CurrentWeapon.WeaponID, out weaponModifiers);

        SO_WeaponBehavior.S_TotalStats totalStats = new SO_WeaponBehavior.S_TotalStats(
            weaponStats: this.CurrentWeapon.WeaponStats,
            weaponModifiers: weaponModifiers,
            ownerStats: this.ownerStats.StatsHandler,
            damageType: CurrentWeapon.DamageType
            );
        CurrentWeapon.WeaponBehavior.Execute(ref attackTransform, ref totalStats);

        CurrentWeapon.Particles?.GetNext(attackTransform.Position, attackTransform.Rotation).PlayParticles();
    }

    protected abstract void OnCooldownEnded();

    public bool SetAllowExecution(bool allow)
        => AllowExecution = allow;
}
