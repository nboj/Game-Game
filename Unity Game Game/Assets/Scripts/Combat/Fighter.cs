using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; 

namespace RPG.Combat {
    public class Fighter : MonoBehaviour {    

    public void Fire(Weapon weapon, Slider weaponReloadSliderUI) {
        weapon.FireWeapon(weaponReloadSliderUI);
    } 
}
}
