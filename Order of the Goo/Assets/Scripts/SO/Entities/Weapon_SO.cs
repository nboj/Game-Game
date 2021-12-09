using UnityEngine;

[CreateAssetMenu(menuName="Weapon_SO", fileName="New Weapon")]
public class Weapon_SO : Entity {
    [Header("Weapon Properties")] 
    [SerializeField] private float weaponRange;
    [SerializeField] private bool hasSplashDamage;
    [SerializeField] private float fireRate;
    [SerializeField] private DiceType damageRollType = DiceType.d6;
    [SerializeField] private CreatureClass weaponClass;
    [SerializeField] private float rollModifier;
    [SerializeField] private float damageModifier;

    public float WeaponRange {
        get => weaponRange;
    }

    public bool HasSplashDamage {
        get => hasSplashDamage;
    }

    public float FireRate {
        get => fireRate;
    } 

    public DiceType DamageRollType {
        get => damageRollType;
    }
}