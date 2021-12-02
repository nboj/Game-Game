using UnityEngine;
 
[CreateAssetMenu(fileName = "New Melee", menuName = "Melee Weapon_SO")]
public class MeleeWeapon_SO : Weapon_SO {
    [SerializeField] private float weaponWidth;
    [SerializeField] private float dexterity = 10f;
    [SerializeField] private float strength = 10f;

    public float WeaponWidth => weaponWidth;
    public float Dexterity => dexterity;
    public float Strength => strength;
} 