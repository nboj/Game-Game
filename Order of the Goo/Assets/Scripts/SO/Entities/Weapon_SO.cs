using UnityEngine;

[CreateAssetMenu(menuName="Weapon_SO", fileName="New Weapon")]
public class Weapon_SO : Entity {
    [Header("Weapon Properties")]
    [SerializeField] private Sprite weaponSprite;
    [SerializeField] private Vector2 damageRange;
    [SerializeField] private float weaponRange;
    [SerializeField] private bool hasSplashDamage;
    [SerializeField] private float fireRate; 

    public Vector2 DamageRange {
        get => damageRange;
    }

    public float WeaponRange {
        get => weaponRange;
    }

    public bool HasSplashDamage {
        get => hasSplashDamage;
    }

    public float FireRate {
        get => fireRate;
    }

    public Sprite WeaponSprite {
        get => weaponSprite;
    }
}