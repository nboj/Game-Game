using System;
using Pathfinding;
using UnityEngine;
using RPG.Control;
using RPG.Combat;

namespace RPG.Core {
    public class AIController : MonoBehaviour {
        [SerializeField] 
        private EnemySO enemy;
        [SerializeField] 
        private bool willStopOnAttack = false;
        [SerializeField]
        private float nextWaypointDistance;

        private Path path;
        private int currentWaypoint;
        private bool reachedEndOfPath = false;
        private int selectedWeaponIndex;
        private Fighter fighter;
        private AIPath aipath;
        private new Rigidbody2D rigidbody;
        private PlayerController player;
        private Seeker seeker;
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
            aipath = GetComponent<AIPath>();
            player = FindObjectOfType<PlayerController>();
            rigidbody = GetComponent<Rigidbody2D>();
            fighter = GetComponent<Fighter>();
            selectedWeaponIndex = 0;
            activeState = EnemyState.IDLE;
            seeker = GetComponent<Seeker>();
            aipath.destination = player.transform.position;
            AutoRepathPolicy policy = new AutoRepathPolicy();
            policy.mode = AutoRepathPolicy.Mode.EveryNSeconds;
            policy.interval = 0.5f;
            aipath.autoRepath = policy;
            InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
        }

        private void OnPathComplete(Path p) {
            if (!p.error) {
                path = p;
                currentWaypoint = 0;
            }
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

        private void FixedUpdate() {
            // if (path == null) { 
            //     return;
            // }
            // if (currentWaypoint >= path.vectorPath.Count) { 
            //     reachedEndOfPath = true; 
            //     return;
            // } else {
            //     reachedEndOfPath = false;
            // }
            //
            // Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidbody.position).normalized;
            // Vector2 force = direction * enemy.EnemySpeed * Time.deltaTime; 
            // rigidbody.velocity = force;
            // float distance = Vector2.Distance(rigidbody.position, path.vectorPath[currentWaypoint]);
            // if (distance < nextWaypointDistance) {
            //     currentWaypoint++;
            // }
        }

        private void UpdatePath() {
            aipath.destination = player.transform.position;
            // if (seeker.IsDone())
            //     seeker.StartPath(rigidbody.position, player.transform.position, OnPathComplete);
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