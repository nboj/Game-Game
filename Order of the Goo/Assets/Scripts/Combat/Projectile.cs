using System;
using UnityEngine;
using System.Collections.Generic;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour {  
        private GameObject parent;
        private RangedWeapon_SO rangedWeapon;
        private Vector2 projectileDirection;
        private Vector2 target;
        private Rigidbody2D rb; 
        private float startRotation;
        private event OnHit onHit;
        private bool destroyAtTarget = false;
        protected bool canControl = true;
        private RangedWeapon_SO weapon;
        private GameObject indicator;

        public virtual void Start() { 
            rb = GetComponent<Rigidbody2D>();
            if (Weapon.UseIndicator)
                indicator = Instantiate(Weapon.IndicatorParticles, target, Quaternion.identity).gameObject;
        }

        public RangedWeapon_SO Weapon {
            get => weapon;
            set => weapon = value;
        }

        public bool DestroyAtTarget {
            get => destroyAtTarget;
            set => destroyAtTarget = value;
        }

        public OnHit OnHit {
            get => onHit;
            set => onHit = value;
        }

        protected Rigidbody2D RB => rb;

        public Vector2 Target {
            get => target;
            set => target = value;
        }    

        public Vector2 ProjectileDirection {
            get => projectileDirection;
            set => projectileDirection = value;
        } 

        public RangedWeapon_SO RangedWeapon {
            set => rangedWeapon = value;
            get => rangedWeapon;
        }

        public GameObject Parent {
            get => parent;
            set => parent = value;
        }

        public virtual void Update() {
            if (!canControl)
                return; 
            UpdatePosition();
            RotateProjectile();
        }

        private void OnBecameInvisible() {
            Destroy(gameObject);
        }

        private void UpdatePosition() { 
            transform.position += new Vector3(projectileDirection.x, projectileDirection.y, 0) * Time.deltaTime * rangedWeapon.ProjectileSpeed;
            if (destroyAtTarget && Vector2.Distance(Target, transform.position) <= 0.2f) {
                OnCollisionEnter2D(null);
            }
        }

        private void RotateProjectile() {
            if (rangedWeapon.HasRotation) {
                transform.Rotate(new Vector3(0, 0, rangedWeapon.RotationSpeed) * Time.deltaTime);
            }
        }

        public virtual void SetRotation(Vector2 target) {
            this.target = target;
            var direction = (target - (Vector2)transform.position).normalized;
            startRotation = rangedWeapon.Projectile.transform.eulerAngles.z;
            projectileDirection = direction.normalized;
            SetObjectRotation(this.projectileDirection, gameObject);
        } 

        private void OnCollisionEnter2D(Collision2D collider) {
            if (collider != null) {
                if (collider.transform == parent.transform || collider.transform.IsChildOf(parent.transform)) {
                    return;
                }
            }

            if (rangedWeapon.SplashRadius > 0) {
                Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, rangedWeapon.SplashRadius); 
                foreach (var child in hit) {
                    var health = child.GetComponent<Health>(); 
                    if (health != null && health.transform != parent.transform && !health.transform.IsChildOf(parent.transform)) {
                        onHit(child.gameObject);
                    } 
                } 
            } else if (collider != null) {
                Health health = collider.gameObject.GetComponent<Health>();
                if (health != null && health.transform != parent.transform && !health.transform.IsChildOf(parent.transform)) { 
                    onHit(health.gameObject);
                }
            } 
            canControl = false;
            PlayDeath();
        }

        private void PlayDeath() {
            if (indicator != null) 
                Destroy(indicator.gameObject);
            gameObject.SetActive(false); 
            var deathParticles =  rangedWeapon.DeathParticles;
            if (deathParticles != null) { 
                var deathVFX = Instantiate(deathParticles, transform.position, Quaternion.identity);
                SetObjectRotation(projectileDirection, deathVFX.gameObject);
                Destroy(deathVFX.gameObject, deathParticles.main.duration);
            }
            Destroy(gameObject);
        }

        protected void SetObjectRotation(Vector2 direction, GameObject go) {
            float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (Math.Abs(rotation) > 360) rotation = 0;
            go.transform.rotation = Quaternion.AngleAxis(rotation + startRotation, Vector3.forward); 
        }

        protected void SetObjectRotation(Vector2 direction) {
            float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (Math.Abs(rotation) > 360) rotation = 0;
            transform.rotation = Quaternion.AngleAxis(rotation + startRotation, Vector3.forward); 
        }

        public virtual void OnDrawGizmos() { 
        }
    }
}