using UnityEngine;
 
[CreateAssetMenu(fileName = "New Melee", menuName = "Melee Weapon_SO")]
public class MeleeWeapon_SO : Weapon_SO {
    [SerializeField] private float weaponWidth; 
    
    public float WeaponWidth => weaponWidth;
} 