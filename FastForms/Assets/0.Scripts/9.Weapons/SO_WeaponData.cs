using StdNounou;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponData", menuName = "Scriptable/Weapons/Data")]
public class SO_WeaponData : ScriptableObject
{
    [field: SerializeField] public string WeaponID { get; private set; }
    [field: SerializeField] public SO_WeaponBehavior WeaponBehavior {  get; private set; }
    [field: SerializeField] public SO_BaseStats WeaponStats {  get; private set; }
    [field: SerializeField] public int InitialProjectilesPoolSize { get; private set; }

    [field: SerializeField] public AnimatorController WeaponAnimController { get; private set; }
    [field: SerializeField] public Sprite WeaponSprite { get; private set; }
    [field: SerializeField] public Vector2 SpritePivotOffset { get; private set; }
    [field: SerializeField] public Vector2 FirePointPosition { get; private set; }
}