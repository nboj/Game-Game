using UnityEngine;

[CreateAssetMenu(menuName="Weapon_SO", fileName="New Weapon")]
public class Weapon_SO : Entity {
    [Header("Weapon Properties")] 
    [SerializeField] private float weaponRange;
    [SerializeField] private bool hasSplashDamage;
    [SerializeField] private float fireRate;

    public float WeaponRange {
        get => weaponRange;
    }

    public bool HasSplashDamage {
        get => hasSplashDamage;
    }

    public float FireRate {
        get => fireRate;
    } 
}