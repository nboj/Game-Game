using UnityEngine;
using Animancer;
 
[CreateAssetMenu(fileName = "New Melee", menuName = "Melee Weapon_SO")]
public class MeleeWeapon_SO : Weapon_SO {
    [SerializeField] private float weaponLength;
    [SerializeField] private float weaponWidth;
    [SerializeField] private float dexterity = 10f;
    [SerializeField] private float strength = 10f;
    [SerializeField] private MixerTransition2D meleeTransition;

    public float WeaponLength => weaponLength;
    public float WeaponWidth => weaponLength;
    public float Dexterity => dexterity;
    public float Strength => strength; 
    public MixerTransition2D MeleeTransition => meleeTransition;
} 