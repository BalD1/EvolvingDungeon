using com.cyborgAssets.inspectorButtonPro;
using StdNounou;
using System;
using UnityEngine;

public abstract class WeaponHandler : MonoBehaviourEventsHandler
{
    [Header("Components")]
    [SerializeField] private GameObject ownerObj;
    protected IComponentHolder owner;
    private MonoStatsHandler ownerStats;

    [SerializeField] protected SpriteRenderer weaponSpriteRenderer;
    [SerializeField] private Transform firePointTransform;
    [SerializeField] private Animator animator;

    public event Action<WeaponHandler> OnCooldownEnded;

    [field: SerializeField] public ObjectRotator Rotator {  get; private set; }
    [field: SerializeField] public SO_WeaponData InitialWeapon { get; private set; } 
    [field: SerializeField, ReadOnly] public SO_WeaponData CurrentWeapon { get; private set; }
    [field: SerializeField, ReadOnly] public SO_EntityWeaponsModifiers OwnerWeaponModifiers { get; private set; }
    private StatsHandler currentWeaponModifiers;

    [SerializeField, ReadOnly] private float cooldown;

    [field: SerializeField, ReadOnly] public bool AllowExecution { get; private set; } = true;

    protected bool isSetup;

    private void Reset()
    {
        Rotator = this.GetComponent<ObjectRotator>();
    }

    protected override void Awake()
    {
        base.Awake();

        owner = ownerObj.GetComponent<IComponentHolder>();

        owner.HolderTryGetComponent(IComponentHolder.E_Component.StatsHandler, out ownerStats);
        IComponentHolder.E_Result tryGetResult = owner.HolderTryGetComponent(IComponentHolder.E_Component.WeaponStatsModifierHandler, out EntityWeaponsModifierHandler handler);
        if (tryGetResult == IComponentHolder.E_Result.Success)
            OwnerWeaponModifiers = handler.Modifiers;
        else
            this.LogError($"Could not get {IComponentHolder.E_Component.WeaponStatsModifierHandler} in holder : {tryGetResult}");
    }

    protected virtual void Start()
    {
        if (InitialWeapon != null) SetNewWeapon(InitialWeapon);
        else if (CurrentWeapon != null) SetNewWeapon(CurrentWeapon);
    }

    protected virtual void Update()
    {
        if (!isSetup) return;
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0) CooldownEnded();
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

        OwnerWeaponModifiers.TryGetStatsModifiersHandler(CurrentWeapon.WeaponID, out currentWeaponModifiers);

        weaponSpriteRenderer.sprite = data.WeaponSprite;
        weaponSpriteRenderer.gameObject.transform.localPosition = data.SpritePivotOffset;
        animator.runtimeAnimatorController = data.WeaponAnimController;

        firePointTransform.localPosition = data.FirePointPosition;

        isSetup = true;
    }

    public void Execute(Vector2 targetPosition)
    {
        if (CurrentWeapon == null) return;
        if (cooldown > 0) return;

        cooldown = GetFinalStat(IStatContainer.E_StatType.AttackCooldown);
        if (cooldown <= 0) cooldown = 0.01f;

        animator.speed = 1/cooldown;
        animator.SetTrigger("Fire");

        Quaternion q = Quaternion.Euler(0, 0, 90);

        SO_WeaponBehavior.S_AttackTransform attackTransform = new SO_WeaponBehavior.S_AttackTransform(
            pos: firePointTransform.position,
            rot: firePointTransform.rotation,
            targPos: targetPosition
            );

        StatsHandler weaponModifiers = null;
        this.OwnerWeaponModifiers.TryGetStatsModifiersHandler(this.CurrentWeapon.WeaponID, out weaponModifiers);

        SO_WeaponBehavior.S_TotalStats totalStats = new SO_WeaponBehavior.S_TotalStats(
            weaponStats: this.CurrentWeapon.WeaponStats,
            weaponModifiers: weaponModifiers,
            ownerStats: this.ownerStats.StatsHandler,
            damageType: CurrentWeapon.DamageType
            );
        CurrentWeapon.WeaponBehavior.Execute(ref attackTransform, ref totalStats, ref CurrentWeapon.WeaponParticles);

        CurrentWeapon.WeaponParticles.FireParticles?.Create(attackTransform.Position, attackTransform.Rotation).PlayParticles();
    }

    public float GetFinalStat(IStatContainer.E_StatType statType)
    {
        float weaponStat = 0;
        float ownerStatValue = 0;
        float modifierValue = 0;

        if (!CurrentWeapon.WeaponStats.TryGetStatValue(statType, out weaponStat)) weaponStat = 0;
        if (currentWeaponModifiers != null)
            if (!currentWeaponModifiers.TryGetFinalStat(statType, out modifierValue)) modifierValue = 0;
        if (!ownerStats.StatsHandler.TryGetFinalStat(statType, out ownerStatValue)) ownerStatValue = 0;

        return weaponStat + ownerStatValue + modifierValue;
    }

    protected virtual void CooldownEnded()
    {
        this.OnCooldownEnded?.Invoke(this);
    }

    public bool SetAllowExecution(bool allow)
        => AllowExecution = allow;
}
