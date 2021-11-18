using System.Collections; 
using UnityEngine; 
using RPG.Core; 

namespace RPG.Combat {
    public class Fighter : MonoBehaviour {
        [SerializeField] WeaponSO[] weapons;
        private int selectedWeaponIndex;

        public WeaponSO[] Weapons {
            get => weapons;
            set => weapons = value;
        }

        public int SelectedWeaponIndex {
            get => selectedWeaponIndex;
            set => selectedWeaponIndex = value;
        }

        private bool[] canFire;

        private void Start() {
            selectedWeaponIndex = 0;
            canFire = new bool[weapons.Length];
            for (int i = 0; i < weapons.Length; i++) {
                canFire[i] = true;
            }
        }

        public void FireWeapon(Vector2 target) {
            if (canFire[SelectedWeaponIndex]) {
                StartCoroutine(WaitToFire(weapons[SelectedWeaponIndex], target));
            }
        }

        private IEnumerator WaitToFire(WeaponSO weapon, Vector2 target) {
            var index = selectedWeaponIndex;
            canFire[index] = false;
            var projectileDirection = (target - new Vector2(transform.position.x, transform.position.y)).normalized;
            var projectileObject = Instantiate(weapon.Projectile, transform.position, Quaternion.identity);
            var projectile = projectileObject.GetComponent<Projectile>();
            var secondsToWait = weapon.ReloadTime;
            projectile.SetRotation(projectileDirection);
            projectile.ProjectileDirection = projectileDirection;
            yield return new WaitForSeconds(secondsToWait);
            canFire[index] = true;
        }
    }
}