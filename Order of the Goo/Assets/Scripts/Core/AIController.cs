using UnityEngine; 
using RPG.Control;

namespace RPG.Core {
    public class AIController : MonoBehaviour { 
        [SerializeField] EnemySO enemy;
        private new Rigidbody2D rigidbody;
        private PlayerController player;

        private void Start() {
            player = FindObjectOfType<PlayerController>();
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update() {
            float playerDistance = Vector2.Distance(player.transform.position, transform.position);
            if (playerDistance <= enemy.ChaseDistance) { 
                Vector2 velocity = (player.transform.position - transform.position).normalized;
                rigidbody.velocity = velocity * enemy.EnemySpeed * Time.deltaTime;
            } else {
                rigidbody.velocity = Vector2.zero;
            }
        }

        private void OnDrawGizmos() { 
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemy.ChaseDistance);
        }
    }
}