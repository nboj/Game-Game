using System.Collections.Generic; 
using UnityEngine; 
 
[RequireComponent(typeof(Fighter))]
public class AggressiveCreature : Creature {
    [SerializeField] private List<Weapon_SO> weapons;
    [SerializeField] private bool canAttack = true; 
    private List<float> reloadDelays;
    private int selectedIndex = 0;
    public virtual bool CanAttack {
        get => canAttack;
        set => canAttack = value;
    }
    private Fighter fighter;

    public Fighter Fighter { 
        get => fighter;
    }

    public override void Awake() {
        base.Awake();
        reloadDelays = new List<float>();
        base.Start(); 
        fighter = GetComponent<Fighter>(); 
        UpdateReloadTimes();
    }

    public override void Start() {
        base.Start();
    } 

    protected internal void UpdateReloadTimes() {
        reloadDelays.Clear();
        for (int i = 0; i < weapons.Count; i++) {
            reloadDelays.Add(Time.time);
        }
    }

    public List<float> ReloadDelays => reloadDelays; 

    protected internal List<Weapon_SO> Weapons {
        get => weapons;
        set => weapons = value;
    }

    protected internal int SelectedIndex {
        get => selectedIndex;
    }

    protected internal void SetSelectedIndex(int index) {
        selectedIndex = index;
    }

    public Weapon_SO GetSelectedWeapon() {
        return weapons[selectedIndex];
    }

    protected internal void Fire(Vector2 target) {
        Debug.Log("FIRED!");
        var weapon = GetSelectedWeapon();
        if (!CanFire() || !CanAttack)
            return;
        reloadDelays[selectedIndex] = Time.time;
        var weaponType = weapon.GetType();
        if (weaponType == typeof(MagicWeapon_SO)) {
            fighter.FireMagic(target, (MagicWeapon_SO)weapon);
        } else if (weaponType == typeof(RangedWeapon_SO)) {
            fighter.FireRanged(target, (RangedWeapon_SO)weapon);
        } else if (weaponType == typeof(MeleeWeapon_SO)) {
            fighter.FireMelee(target, (MeleeWeapon_SO)weapon);
        }
    }

    protected internal void Fire(Vector2 target, Vector2 startPos) {
        Debug.Log("FIRED!");
        var weapon = GetSelectedWeapon();
        if (!CanFire() || !CanAttack)
            return;
        reloadDelays[selectedIndex] = Time.time;
        var weaponType = weapon.GetType();
        if (weaponType == typeof(MagicWeapon_SO)) {
            fighter.FireMagic(target, (MagicWeapon_SO)weapon, startPos);
        } else if (weaponType == typeof(RangedWeapon_SO)) {
            fighter.FireRanged(target, (RangedWeapon_SO)weapon, startPos);
        } else if (weaponType == typeof(MeleeWeapon_SO)) {
            fighter.FireMelee(target, (MeleeWeapon_SO)weapon);
        }
    }

    protected internal void Fire(Vector2 target, Vector2 startPos, bool destroyAtTarget = false) {
        Debug.Log("FIRED!");
        var weapon = GetSelectedWeapon();
        if (!CanFire() || !CanAttack)
            return;
        reloadDelays[selectedIndex] = Time.time;
        var weaponType = weapon.GetType();
        if (weaponType == typeof(MagicWeapon_SO)) {
            fighter.FireMagic(target, (MagicWeapon_SO)weapon, startPos, destroyAtTarget);
        } else if (weaponType == typeof(RangedWeapon_SO)) {
            fighter.FireRanged(target, (RangedWeapon_SO)weapon, startPos, destroyAtTarget);
        } else if (weaponType == typeof(MeleeWeapon_SO)) {
            fighter.FireMelee(target, (MeleeWeapon_SO)weapon);
        }
    }

    protected internal override void SetEnabled(bool value) {
        base.SetEnabled(value);
        CanAttack = value;
    }

    private bool CanFire() {
        var currentWeaponTime = reloadDelays[selectedIndex];
        var selectedWeapon = GetSelectedWeapon();
        return Time.time - currentWeaponTime >= selectedWeapon.FireRate;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        var RangedWeapon = GetSelectedWeapon() as RangedWeapon_SO;
        Gizmos.DrawWireSphere(transform.position,  RangedWeapon.SplashRadius);
    }
} 