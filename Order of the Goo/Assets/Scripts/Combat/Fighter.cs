using RPG.Combat; 
using UnityEngine; 

public class Fighter : MonoBehaviour {
    public void FireRanged(Vector2 target, RangedWeapon_SO weapon) {
        var projectileObject = Instantiate(weapon.Projectile, transform.position, weapon.Projectile.transform.rotation);
        var projectile = projectileObject.GetComponent<Projectile>();
        SetupProjectile(projectile, weapon, target);
        Debug.Log("Fired Ranged");
    }
    
    public void FireMagic(Vector2 target, MagicWeapon_SO weapon) {
        if (weapon.RangedMagic) {
            var projectileObject = Instantiate(weapon.MagicProjectile, transform.position, weapon.MagicProjectile.transform.rotation);
            var projectile = projectileObject.GetComponent<MagicProjectile>();
            projectile.MagicWeapon = weapon;
            SetupProjectile(projectile, weapon, target);
            projectile.SetupTracking();
        }
        Debug.Log("Fired Magic");
    }
    
    public void FireMelee(Vector2 target, MeleeWeapon_SO weapon) {
        Debug.Log("Fired Melee");
    } 
    
    private float GetDamage(Vector2 damageRange) {
        return Random.Range(damageRange.x, damageRange.y);
    }

    private void SetupProjectile(Projectile projectile, RangedWeapon_SO weapon, Vector2 target) { 
        projectile.Parent = gameObject;
        projectile.CalculateDamage = ()=>GetDamage(weapon.DamageRange);
        projectile.RangedWeapon = weapon; 
        projectile.SetRotation(target);
    }
} 