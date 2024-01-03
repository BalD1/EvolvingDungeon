using com.cyborgAssets.inspectorButtonPro;
using StdNounou;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponData", menuName = "Scriptable/Weapons/Data")]
public class SO_WeaponData : ScriptableObject
{
    [field: SerializeField] public string WeaponID { get; private set; }
    [field: SerializeField] public SO_WeaponBehavior WeaponBehavior {  get; private set; }
    [field: SerializeField] public SO_BaseStats WeaponStats {  get; private set; }
    [field: SerializeField] public IDamageable.E_DamagesType DamageType { get; private set; }

    [field: SerializeField] public S_Particles WeaponParticles;

    [field: SerializeField] public int InitialProjectilesPoolSize { get; private set; }

    [field: SerializeField] public RuntimeAnimatorController WeaponAnimController { get; private set; }
    [field: SerializeField] public Sprite WeaponSprite { get; private set; }
    [field: SerializeField] public Vector2 SpritePivotOffset { get; private set; }
    [field: SerializeField] public Vector2 FirePointPosition { get; private set; }

    [System.Serializable]
    public struct S_Particles
    {
        [field: SerializeField] public ParticlesPlayer FireParticles { get; private set; }
        [field: SerializeField] public ParticlesPlayer ImpactParticles { get; private set; }
        [field: SerializeField] public ParticlesPlayer EntityHitParticles { get; private set; }
    }

    [field: SerializeField] public SO_StringFormat DescriptionFormat { get; private set; }
    [field: SerializeField, TextArea(10, 1000)] public string Description { get; private set; }
    [field: SerializeField, TextArea(10, 1000)] public string RichDescription { get; private set; }

    private void OnValidate()
    {
        BuildDescription();
    }

    [ProButton]
    private void BuildDescription()
    {
        if (DescriptionFormat == null || DescriptionFormat.Format == "") return;
        WeaponStats.TryGetStatValue(IStatContainer.E_StatType.BaseDamages, out float damages);
        WeaponStats.TryGetStatValue(IStatContainer.E_StatType.AttackCooldown, out float cooldown);
        WeaponStats.TryGetStatValue(IStatContainer.E_StatType.CritChances, out float critChances);
        WeaponStats.TryGetStatValue(IStatContainer.E_StatType.CritMultiplier, out float critMultiplier);

        Description = string.Format(DescriptionFormat.Format, damages, cooldown, critChances, critMultiplier, DamageType);

        string typeColor = "white";
        switch (DamageType)
        {
            case IDamageable.E_DamagesType.Blunt:
                typeColor = "#b5bbc7"; //silver
                break;

            case IDamageable.E_DamagesType.Sharp:
                typeColor = "#fddb6d"; //yellow
                break;

            case IDamageable.E_DamagesType.Flames:
                typeColor = "#e25822"; //orange
                break;
        }
        RichDescription = string.Format(DescriptionFormat.RichFormat, damages, cooldown, critChances, critMultiplier, typeColor, DamageType);

#if UNITY_EDITOR
        Undo.RecordObject(this, "Changed description");
#endif
    }
}