using System;
using UnityEngine; 

namespace RPG.Combat {
    public class Projectile : MonoBehaviour {  
        private GameObject parent;
        private RangedWeapon_SO rangedWeapon;
        private Vector2 projectileDirection;
        private Vector2 target;
        private Rigidbody2D rb; 
        private float startRotation;
        private event OnHit onHit;

        public virtual void Start() { 
            rb = GetComponent<Rigidbody2D>();
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
            UpdatePosition();
            RotateProjectile();
        }

        private void UpdatePosition() { 
            transform.position += new Vector3(projectileDirection.x, projectileDirection.y, 0) * Time.deltaTime * rangedWeapon.ProjectileSpeed;
        }

        private void RotateProjectile() {
            if (rangedWeapon.HasRotation) {
                transform.Rotate(new Vector3(0, 0, rangedWeapon.RotationSpeed));
            }
        }

        public virtual void SetRotation(Vector2 target) {
            this.target = target;
            var direction = (target - (Vector2)transform.position).normalized;
            startRotation = rangedWeapon.Projectile.transform.eulerAngles.z;
            projectileDirection = direction.normalized;
            SetObjectRotation(this.projectileDirection, gameObject);
        }

        private void OnBecameInvisible() {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collider) {
            if (collider.gameObject == parent)
                return;

            if (rangedWeapon.SplashRadius > 0) {
                Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, rangedWeapon.SplashRadius);
                for (int i = 0; i < hit.Length; i++) {
                    var health = hit[i].GetComponent<Health>();
                    if (health != null && health.gameObject != parent) {
                        onHit(hit[i].gameObject);
                    }
                } 
            } else {
                Health health = collider.gameObject.GetComponent<Health>();
                if (health != null) {
                    onHit(health.gameObject);
                }
            }
            PlayDeath();
        }

        private void PlayDeath() {
            GetComponent<Renderer>().enabled = false;
            var deathParticles =  rangedWeapon.DeathParticles;
            if (deathParticles != null) { 
                var deathVFX = Instantiate(deathParticles, transform.position, Quaternion.identity);
                SetObjectRotation(projectileDirection, deathVFX.gameObject);
                Destroy(deathVFX.gameObject, deathParticles.main.duration);
            }
            Destroy(gameObject, deathParticles.main.duration);
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