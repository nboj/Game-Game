using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour { 
    [BoxGroup("Projectile Properties", centerLabel:true)]
    [SerializeField] float _projectileSpeed; 
    [BoxGroup("Projectile Properties", centerLabel:true)]
    [SerializeField] float _projectileDamage; 
    [BoxGroup("Other Properties", centerLabel:true)]
    [SerializeField] bool _hasRotation;  
    [BoxGroup("Other Properties", centerLabel:true)]
    [SerializeField] float _rotationAmount; 
    private Vector2 _projectileDirection;
    public Vector2 ProjectileDirection {get => _projectileDirection; set => _projectileDirection = value;}  
 
    private void Update() {
        transform.position += new Vector3(_projectileDirection.x, _projectileDirection.y, 0) * Time.deltaTime * _projectileSpeed;
        if (_hasRotation) {
            transform.Rotate(0, 0, _rotationAmount * Time.deltaTime);
        }
    }

    public void SetRotation(Vector2 direction) { 
        direction = direction.normalized;
        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (rotation < 0) rotation += 360; 
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + rotation);
    }

    void OnBecameInvisible() {   
        Destroy(gameObject);
    }
}

}