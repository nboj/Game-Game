using RPG.Combat; 
using UnityEngine;

public delegate void OnHit(GameObject go);

public class Fighter : MonoBehaviour {
    public event OnHit OnHit;

    private void Start() {
        OnHit += HitController;
    }

    public void FireRanged(Vector2 target, RangedWeapon_SO weapon) {
        var projectileObject = Instantiate(weapon.Projectile, transform.position, weapon.Projectile.transform.rotation);
        var projectile = projectileObject.GetComponent<Projectile>();
        SetupProjectile(projectile, weapon, target); 
    }
    
    public void FireMagic(Vector2 target, MagicWeapon_SO weapon) {
        if (weapon.RangedMagic) {
            var projectileObject = Instantiate(weapon.MagicProjectile, transform.position, weapon.MagicProjectile.transform.rotation);
            var projectile = projectileObject.GetComponent<MagicProjectile>();
            projectile.MagicWeapon = weapon;
            SetupProjectile(projectile, weapon, target);
            projectile.SetupTracking();
        } 
    }
    
    public void FireMelee(Vector2 target, MeleeWeapon_SO weapon) { 
    } 
    
    private void GetDamage(Creature_SO creature) { 
    }

    private void SetupProjectile(Projectile projectile, RangedWeapon_SO weapon, Vector2 target) { 
        projectile.Parent = gameObject; 
        projectile.RangedWeapon = weapon; 
        projectile.OnHit = OnHit; 
        projectile.SetRotation(target);
    }

    private void HitController(GameObject other) {
        var enemy = other.GetComponent<Creature>();
        var creature = GetComponent<Creature>();
        var enemyAC = enemy.CreatureSO.AC;
        if (creature != null) {
            var maxValue = creature.GetDiceValue(creature.CreatureSO.DiceType); 
            var totalDamage = 0f;
            for (int i = 0; i < creature.CreatureSO.Level; i++) {
                totalDamage += creature.RollDie(maxValue);
            }
            var rawModifier = creature.CreatureSO.Strength - 10;
            var modifier = rawModifier / 2;
            totalDamage += rawModifier != 0 ? modifier : 0; 
            var hitRoll = creature.RollDie(20);
            if (hitRoll == 1) {
                // hurt self
                GetComponent<Health>().TakeDamage(totalDamage);
            } else if (hitRoll >= enemyAC && hitRoll < 20) {
                // hurt enemy
                other.GetComponent<Health>().TakeDamage(totalDamage);
            } else if (hitRoll >= 20) {
                // crit enemy
                other.GetComponent<Health>().TakeDamage(totalDamage * 2);
            }
            // else missed 
            Debug.Log(totalDamage);
        }
    }
} 