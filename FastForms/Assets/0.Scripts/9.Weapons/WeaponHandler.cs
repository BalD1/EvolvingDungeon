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

    private S_WeaponData weaponResultData;

    public struct S_WeaponData
    {
        public S_WeaponData(SO_BaseStats.E_Team team, IDamageable.E_DamagesType damagesType, float damages, float critChances,
                            float critMultiplier, float speed, float cooldown, float piercingValue)
        {
            this.team = team;
            this.damagesType = damagesType;
            this.damages = damages;
            this.critChances = critChances;
            this.critMultiplier = critMultiplier;
            this.speed = speed;
            this.cooldown = cooldown;
            this.piercingValue = piercingValue;
        }
        public SO_BaseStats.E_Team team;
        public IDamageable.E_DamagesType damagesType;
        public float damages;
        public float critChances;
        public float critMultiplier;
        public float speed;
        public float cooldown;
        public float piercingValue;

    }

    protected override void Awake()
    {
        base.Awake();

        owner = ownerObj.GetComponent<IComponentHolder>();
        
        owner.HolderTryGetComponent(IComponentHolder.E_Component.StatsHandler, out ownerStats);
        owner.HolderTryGetComponent(IComponentHolder.E_Component.WeaponStatsModifierHandler, out EntityWeaponsModifierHandler handler);
        OwnerWeaponModifiers = handler.Modifiers;
        SetNewWeapon(InitialWeapon);

        weaponResultData.team = ownerStats.StatsHandler.GetTeam();
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

        this.weaponResultData.damagesType = data.DamageType;

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
        SetWeaponResultData();
        CurrentWeapon.WeaponBehavior.Execute(firePointTransform.position, firePointTransform.rotation, MouseUtils.GetMouseWorldPosition(), weaponResultData);
    }

    private void SetWeaponResultData()
    {
        weaponResultData.damages = GetFinalStat(IStatContainer.E_StatType.BaseDamages);
        weaponResultData.critChances = GetFinalStat(IStatContainer.E_StatType.CritChances);
        weaponResultData.critMultiplier = GetFinalStat(IStatContainer.E_StatType.CritMultiplier);
        weaponResultData.speed = GetFinalStat(IStatContainer.E_StatType.Speed);
        weaponResultData.cooldown = GetFinalStat(IStatContainer.E_StatType.AttackCooldown);
        weaponResultData.piercingValue = GetFinalStat(IStatContainer.E_StatType.Piercing);
    }

    private float GetFinalStat(IStatContainer.E_StatType statType)
    {
        float weaponStat = 0;
        float ownerStatValue = 0;
        float modifierValue = 0;

        if (!CurrentWeapon.WeaponStats.TryGetStatValue(statType, out weaponStat)) weaponStat = 0;

        if (OwnerWeaponModifiers.TryGetStatsModifiersHandler(CurrentWeapon.WeaponID, out StatsHandler stats))
            if (!stats.TryGetFinalStat(statType, out modifierValue)) modifierValue = 0;

        if (!ownerStats.StatsHandler.TryGetFinalStat(statType, out ownerStatValue)) ownerStatValue = 0;

        return weaponStat + ownerStatValue + modifierValue;
    }

    protected abstract void OnCooldownEnded();

    public bool SetAllowExecution(bool allow)
        => AllowExecution = allow;
}
