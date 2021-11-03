using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour {
    [SerializeField] GameObject projectileObject; 
    private float _timeLeft;
    private bool _canFire = true;
    public float TimeLeft {get=>_timeLeft; set=>_timeLeft = value;}

    public void FireWeapon(Transform weaponParent) { 
        if (_canFire)
            StartCoroutine(WaitToFire(weaponParent));
    }

    private IEnumerator WaitToFire(Transform weaponParent) {
        _canFire = false;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 projectileDirection = (mousePos - new Vector2(weaponParent.position.x, weaponParent.position.y) ).normalized;
        GameObject projectileObject = Instantiate(this.projectileObject, weaponParent.position, Quaternion.identity); 
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        float secondsToWait = projectile.TimeBetweenAttacks;
        projectile.SetRotation(projectileDirection);
        projectile.ProjectileDirection = projectileDirection;
        yield return new WaitForSeconds(secondsToWait);
        _canFire = true;
    }
}
