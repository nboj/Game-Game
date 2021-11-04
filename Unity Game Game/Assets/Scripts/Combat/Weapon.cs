using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RPG.Combat {
    public class Weapon : MonoBehaviour {
        [SerializeField] GameObject _projectileObject;   
        [SerializeField] float _timeBetweenAttacks; 
        private bool _canFire = true;
        private float _startTime;
        private Slider _timeSlider; 

        private void Update() {
            if(_timeSlider != null) {
                _timeSlider.value = Time.time - _startTime; 
            }
        }

        public void FireWeapon(Slider timeSlider) { 
            if (_canFire) {
                _timeSlider = timeSlider;
                StartCoroutine(WaitToFire());
            }
        } 

        private IEnumerator WaitToFire() {
            _canFire = false;
            _startTime = Time.time;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 projectileDirection = (mousePos - new Vector2(transform.position.x, transform.position.y) ).normalized;
            GameObject _projectileObject = Instantiate(this._projectileObject, transform.position, Quaternion.identity); 
            Projectile projectile = _projectileObject.GetComponent<Projectile>();
            float secondsToWait = _timeBetweenAttacks;
            projectile.SetRotation(projectileDirection);
            projectile.ProjectileDirection = projectileDirection;
            _timeSlider.maxValue = secondsToWait;  
            yield return new WaitForSeconds(secondsToWait);
            _canFire = true;
        }
    } 
}
