using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; 
using RPG.Core;
using RPG.Control;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour {     
        private PlayerController player;
        private bool[] canFire;

        private void Start() {
            player = GetComponentInParent<PlayerController>();
            canFire = new bool[player.WeaponsArrayLength];
            for (int i = 0; i < canFire.Length; i++) {
                canFire[i] = true;
            }
        }

        public void FireWeapon(WeaponSO weapon) { 
            if (canFire[player.SelectedIndex]) { 
                StartCoroutine(WaitToFire(weapon));
            }
        } 

        private IEnumerator WaitToFire(WeaponSO weapon) {
            int selectedIndex = player.SelectedIndex;
            canFire[selectedIndex] = false; 
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 projectileDirection = (mousePos - new Vector2(transform.position.x, transform.position.y) ).normalized;
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
