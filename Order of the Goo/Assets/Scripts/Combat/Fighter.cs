using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; 
using RPG.Core;
using RPG.Control;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour {     
        [SerializeField] WeaponSO[] weapons; 
        public WeaponSO[] Weapons { get => weapons; set => weapons = value; }
        private bool[] canFire;

        private void Start() { 
            canFire = new bool[weapons.Length];
            for (int i = 0; i < weapons.Length; i++) {
                canFire[i] = true;
            }
        }

        public void FireWeapon(Vector2 target, int selectedIndex) { 
            if (canFire[selectedIndex]) { 
                StartCoroutine(WaitToFire(weapons[selectedIndex], target, selectedIndex));
            }
        } 

        private IEnumerator WaitToFire(WeaponSO weapon, Vector2 target, int selectedIndex) { 
            canFire[selectedIndex] = false; 
            Vector2 projectileDirection = (target - new Vector2(transform.position.x, transform.position.y) ).normalized;
            GameObject _projectileObject = Instantiate(weapon.Projectile, transform.position, Quaternion.identity); 
            Projectile projectile = _projectileObject.GetComponent<Projectile>();
            float secondsToWait = weapon.ReloadTime;
            projectile.SetRotation(projectileDirection);
            projectile.ProjectileDirection = projectileDirection; 
            yield return new WaitForSeconds(secondsToWait);
            canFire[selectedIndex] = true;
        }
    }
}
