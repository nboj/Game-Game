using RPG.Combat;
using System.Collections;
using TMPro;
using UnityEngine;
using Animancer;
using Animancer.FSM;

public delegate void OnHit(GameObject go);

public class Fighter : MonoBehaviour {
    [SerializeField] private GameObject damageText;
    [SerializeField] private Vector3 weaponOffeset;  
    public event OnHit OnHit;
    private Animator animator;
#if UNITY_EDITOR
    [SerializeField] float weaponWidth;
    [SerializeField] float weaponLength;
#endif

    private void Start() {  
        OnHit += HitController;
        animator = GetComponentInChildren<Animator>();
    }

    public void InvokeOnHit(GameObject go) {
        OnHit(go);
    }

    public void FireRanged(Vector2 target, RangedWeapon_SO weapon, bool destroyAtTarget = false) {
        var projectileObject = Instantiate(weapon.Projectile, transform.position, weapon.Projectile.transform.rotation);
        var projectile = projectileObject.GetComponent<Projectile>();
        SetupProjectile(projectile, weapon, target, destroyAtTarget);
    }

    public void FireRanged(Vector2 target, RangedWeapon_SO weapon, Vector2 startPos, bool destroyAtTarget = false) {
        var projectileObject = Instantiate(weapon.Projectile, startPos, weapon.Projectile.transform.rotation);
        var projectile = projectileObject.GetComponent<Projectile>();
        SetupProjectile(projectile, weapon, target, destroyAtTarget);
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

    public void FireMagic(Vector2 target, MagicWeapon_SO weapon, Vector2 startPos, bool destroyAtTarget = false) {
        if (weapon.RangedMagic) {
            var projectileObject = Instantiate(weapon.MagicProjectile, startPos, weapon.MagicProjectile.transform.rotation);
            var projectile = projectileObject.GetComponent<MagicProjectile>();
            projectile.MagicWeapon = weapon;
            SetupProjectile(projectile, weapon, target, destroyAtTarget);
            projectile.SetupTracking();
        }
    }

    public void FireMelee(Vector2 target, MeleeWeapon_SO weapon) {
        float angle = GetLookatAngle(target);
        var dir = (target - (Vector2)transform.position).normalized;
        animator.runtimeAnimatorController = weapon.OverrideAnimator;
        if (weapon.HasSplashDamage) {
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position + new Vector3(weapon.WeaponRange / 2f, 0f), new Vector2(weapon.WeaponLength, weapon.WeaponWidth), angle);
            foreach (var collider in hitColliders) {  
                var health = collider.GetComponent<Health>();
                if (health != null && health.transform != transform) {
                    InvokeOnHit(health.gameObject);
                }
            }
        } else {
            Collider2D hitCollider = Physics2D.OverlapBox(transform.position + new Vector3(weapon.WeaponRange / 2f, 0f), new Vector2(weapon.WeaponLength, weapon.WeaponWidth), angle);
            var health = hitCollider.GetComponent<Health>();
            if (health != null && health.transform != transform) { 
                InvokeOnHit(health.gameObject);
            }
        }
    }  

    private float GetLookatAngle(Vector2 target) {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle;
    }

    private void SetupProjectile(Projectile projectile, RangedWeapon_SO weapon, Vector2 target, bool destroyAtTarget = false) { 
        projectile.Parent = gameObject;
        projectile.transform.position += new Vector3(0, 1f, 0);
        projectile.RangedWeapon = weapon; 
        projectile.OnHit = OnHit; 
        projectile.SetRotation(target);
        projectile.DestroyAtTarget = destroyAtTarget;
        projectile.Weapon = weapon;
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
        var textOb = Instantiate(damageText, go.transform.position, Quaternion.identity, go.transform);
        textOb.GetComponent<RectTransform>().transform.position = go.transform.position;
        var text = textOb.GetComponent<TextMeshPro>();
        var anim = textOb.GetComponent<Animator>();
        text.text = totalDamage.ToString();
        anim.SetTrigger("Hit");
        var duration = anim.GetCurrentAnimatorStateInfo(0).length; 
        yield return new WaitForSeconds(duration);
        Destroy(textOb); 
    }

    private void OnDrawGizmos() {
#if UNITY_EDITOR
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(weaponLength/2f, 0f) + weaponOffeset, new Vector2(weaponLength, weaponWidth));
#endif
    }
} 