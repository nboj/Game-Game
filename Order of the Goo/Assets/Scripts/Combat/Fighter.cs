using RPG.Combat; 
using UnityEngine;
using TMPro;
using System.Collections;

public delegate void OnHit(GameObject go);

public class Fighter : MonoBehaviour {
    [SerializeField] private GameObject damageText;
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
        projectile.transform.position += new Vector3(0, 1f, 0);
        projectile.RangedWeapon = weapon; 
        projectile.OnHit = OnHit; 
        projectile.SetRotation(target);
    }

    private void HitController(GameObject other) {
        var enemy = other.GetComponent<AggressiveCreature>();
        var creature = GetComponent<AggressiveCreature>();
        var enemyAC = enemy.CreatureSO.AC; 
        float GetDamageFromDice(DiceType diceType, float multiplier) {
            var max = creature.GetDiceValue(diceType);
            var damage = 0f;
            for (var i = 0; i < multiplier; i++) {
                damage += creature.RollDie(max);
            }
            return damage;
        }
        if (creature != null) { 
            var totalDamage = 0f;
            totalDamage += GetDamageFromDice(creature.Weapons[creature.SelectedIndex].DamageRollType, creature.CreatureSO.Level);
            var rawModifier = creature.CreatureSO.Strength - 10;
            var modifier = (int)(rawModifier / 2);
            totalDamage += rawModifier != 0 ? modifier : 0;
            var totalHitRoll = 0f;
            var hitRoll = creature.RollDie(20);
            totalHitRoll += hitRoll;
            if (totalHitRoll == 1) {// Separate the checking when casting the shot
                // hurt self
                other.GetComponent<Health>().TakeDamage(totalDamage / 2);
            } else if (totalHitRoll > enemyAC && hitRoll != 20) {
                // hurt enemy
                other.GetComponent<Health>().TakeDamage(totalDamage);
            } else if (hitRoll == 20) {
                // crit enemy
                // check another hit and if hit then double it + original damage amount
                other.GetComponent<Health>().TakeDamage(totalDamage * 2);
            }

            // 
            // else missed
            StartCoroutine(PlayHit(totalDamage, other));
            Debug.Log(totalDamage);
        }
    }

    private IEnumerator PlayHit(float totalDamage, GameObject go) { 
        var textOb = Instantiate(damageText, go.transform.position, Quaternion.identity); 
        var text = textOb.GetComponent<TextMeshPro>();
        var anim = textOb.GetComponent<Animator>();
        text.text = totalDamage.ToString();
        anim.SetTrigger("Hit");
        var duration = anim.GetCurrentAnimatorStateInfo(0).length; 
        yield return new WaitForSeconds(duration);
        Destroy(textOb);
        
    }
} 