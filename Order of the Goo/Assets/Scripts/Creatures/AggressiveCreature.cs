using System.Collections.Generic; 
using UnityEngine;
 
[RequireComponent(typeof(Fighter))]
public class AggressiveCreature : Creature {
    [SerializeField] private List<Weapon_SO> weapons;
    private List<float> reloadDelays;
    private int selectedIndex = 0;
    private bool canAttack = true;
    private Fighter fighter;

    public override void Start() {
        reloadDelays = new List<float>();
        base.Start(); 
        fighter = GetComponent<Fighter>(); 
        UpdateReloadTimes();
    } 

    protected internal void UpdateReloadTimes() {
        reloadDelays.Clear();
        for (int i = 0; i < weapons.Count; i++) {
            reloadDelays.Add(Time.time);
        }
    }

    public List<float> ReloadDelays => reloadDelays;
    public bool CanAttack => canAttack; 

    protected internal List<Weapon_SO> Weapons {
        get => weapons;
    }

    protected internal int SelectedIndex {
        get => selectedIndex;
    }

    protected internal void SetSelectedIndex(int index) {
        selectedIndex = index;
    }

    protected internal Weapon_SO GetSelectedWeapon() {
        return weapons[selectedIndex];
    } 

    protected internal void Fire(Vector2 target) {
        var weapon = GetSelectedWeapon();
        if (!CanFire())
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

    protected internal override void SetEnabled(bool value) {
        base.SetEnabled(value);
        fighter.enabled = value;
    }

    private bool CanFire() {
        var currentWeaponTime = reloadDelays[selectedIndex];
        var selectedWeapon = GetSelectedWeapon();
        return Time.time - currentWeaponTime >= selectedWeapon.FireRate;
    }
} 