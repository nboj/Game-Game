using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {
    [SerializeField] GameObject _projectileObject;   
    private bool _canFire = true;
    private float _startTime;
    private Slider _timeSlider; 

    private void Update() {
        if(_timeSlider != null) {
            _timeSlider.value = Time.time - _startTime; 
        }
    }

    public void FireWeapon(Transform weaponParent, Slider timeSlider) { 
        if (_canFire) {
            _timeSlider = timeSlider;
            StartCoroutine(WaitToFire(weaponParent));
        }
    } 

    private IEnumerator WaitToFire(Transform weaponParent) {
        _canFire = false;
        _startTime = Time.time;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 projectileDirection = (mousePos - new Vector2(weaponParent.position.x, weaponParent.position.y) ).normalized;
        GameObject _projectileObject = Instantiate(this._projectileObject, weaponParent.position, Quaternion.identity); 
        Projectile projectile = _projectileObject.GetComponent<Projectile>();
        float secondsToWait = projectile.TimeBetweenAttacks;
        projectile.SetRotation(projectileDirection);
        projectile.ProjectileDirection = projectileDirection;
        _timeSlider.maxValue = secondsToWait;  
        yield return new WaitForSeconds(secondsToWait);
        _canFire = true;
    }
}
