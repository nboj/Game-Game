using RPG.Combat;
using System.Collections;
using TMPro;
using UnityEngine;

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
        var enemyAC = creature.CreatureSO.AC;  
        if (creature != null) {
            var weapon = creature.Weapons[creature.SelectedIndex];
            var classModValue = GameController.GetModifierValue(weapon.DamageClass);
            // get damage 
            var totalDamage = GameController.RollDie(weapon.DiceMultiplier, GameController.GetDiceValue(weapon.DamageRollType)) + classModValue + weapon.DamageModifier + (classModValue * weapon.DamageClassMultiplier); 
            // roll to hit
            var totalHitRoll = 0f;
            var hitRoll = GameController.RollDie(1, 20);
            totalHitRoll += hitRoll + weapon.AttackModifier + classModValue;
            var resistancePercent = enemy.CreatureSO.ResistancePercent;
            if (totalHitRoll == 1) {
                // hit with full resistance %  
                totalDamage = (totalDamage * resistancePercent) / 100f; 
            } else if (hitRoll == 20) {
                // Hit without resistance % * 2
                Debug.Log("Resistance: Crit");
                totalDamage = ((totalDamage * resistancePercent) / 100f) * 2;  
            } else { 
                var resistanceReduction = Mathf.Clamp(totalHitRoll, 1, 100 - enemyAC - resistancePercent);// here
                Debug.Log(resistanceReduction);
                var damagePreCalculation = totalDamage * (resistancePercent + resistanceReduction); 
                totalDamage = (damagePreCalculation) / 100f;
            }
            totalDamage = Mathf.Ceil(totalDamage);
            other.GetComponent<Health>().TakeDamage(totalDamage);
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