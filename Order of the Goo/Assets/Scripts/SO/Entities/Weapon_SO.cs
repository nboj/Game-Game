using UnityEngine;

[CreateAssetMenu(menuName="Weapon_SO", fileName="New Weapon")]
public class Weapon_SO : Entity {
    [Header("Weapon Properties")] 
    [SerializeField] private float weaponRange;
    [SerializeField] private bool hasSplashDamage = false;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float diceMultiplier = 1f;
    [SerializeField] private DiceType damageRollType = DiceType.d6;
    [SerializeField] private CreatureClass damageClass = CreatureClass.WIZARD;
    [SerializeField] private float damageClassMultiplier = 1f;
    [SerializeField] private float attackModifier = 0f;
    [SerializeField] private float damageModifier = 0f;

    public float WeaponRange {
        get => weaponRange;
    }

    public bool HasSplashDamage {
        get => hasSplashDamage;
    }

    public float FireRate {
        get => fireRate;
    } 
    public float DiceMultiplier {
        get => diceMultiplier;
    }

    public DiceType DamageRollType {
        get => damageRollType;
    }

    public CreatureClass DamageClass {
        get => damageClass;
    }

    public float AttackModifier {
        get => attackModifier;
    }

    public float DamageModifier {
        get => damageModifier;
    }

    public float DamageClassMultiplier {
        get => damageClassMultiplier;
    }
}