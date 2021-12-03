using RPG.Combat;
using UnityEngine;
 
[CreateAssetMenu(fileName = "New Ranged Weapon_SO", menuName = "Ranged Weapon_SO")]
public class RangedWeapon_SO : Weapon_SO {
    [Header("Ranged Properties")]
    [SerializeField] private float splashRadius;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private bool hasRotation;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private ParticleSystem deathParticles;
    
    public GameObject Projectile => projectile;
    public float ProjectileSpeed => projectileSpeed;
    public bool HasRotation => hasRotation;
    public float RotationSpeed => rotationSpeed;
    public ParticleSystem DeathParticles => deathParticles;
    public float SplashRadius => splashRadius; 
}