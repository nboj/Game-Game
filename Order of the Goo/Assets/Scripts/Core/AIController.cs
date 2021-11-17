using UnityEngine; 
using RPG.Control;
using RPG.Combat;

namespace RPG.Core {
    public class AIController : MonoBehaviour { 
        [SerializeField] EnemySO enemy;
        [SerializeField] bool willStopOnAttack = false;
        private int selectedWeaponIndex;
        private Fighter fighter;
        private new Rigidbody2D rigidbody;
        private PlayerController player;
        EnemyState activeState; 
        public EnemyState ActiveState { get => activeState; set => activeState = value; }
        public int SelectedIndex { get => selectedWeaponIndex; }
        public PlayerController Player { get => player; }   
        public enum EnemyState {
            CHASE,
            ATTACK,
            RUN,
            IDLE
        } 

        private void Start() {
            player = FindObjectOfType<PlayerController>();
            rigidbody = GetComponent<Rigidbody2D>();
            fighter = GetComponent<Fighter>();
            selectedWeaponIndex = 0;
            activeState = EnemyState.IDLE;  
        }

        private void Update() {
            float playerDistance = Vector2.Distance(player.transform.position, transform.position);
            if (playerDistance <= enemy.ChaseDistance && playerDistance > enemy.AttackDistance) { 
                //MoveToward(player.transform.position);
                activeState = EnemyState.CHASE;
            } else if (playerDistance <= enemy.AttackDistance && playerDistance > enemy.RepelDistance) {
                activeState = EnemyState.ATTACK;
                // if (willStopOnAttack) {
                //     StopMoving();
                // } 
                // fighter.FireWeapon(player.transform.position, selectedWeaponIndex);
            } else if (playerDistance <= enemy.RepelDistance) {
                activeState = EnemyState.RUN;
                // MoveAway(player.transform.position);
            } else {
                activeState = EnemyState.IDLE;
                // StopMoving();
            }
        }

        private void StopMoving() {
            rigidbody.velocity = Vector2.zero;
        }

        private void MoveToward(Vector3 position) {
            Vector2 velocity = (position - transform.position).normalized;
            rigidbody.velocity = velocity * enemy.EnemySpeed * Time.deltaTime;
        }

        private void MoveAway(Vector3 position) { 
            Vector2 velocity = (transform.position - position).normalized;
            rigidbody.velocity = velocity * enemy.EnemySpeed * Time.deltaTime;
        }

        private void OnDrawGizmos() { 
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, enemy.ChaseDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemy.AttackDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, enemy.RepelDistance);
        }
    }
}