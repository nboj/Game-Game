using System;
using Pathfinding;
using UnityEngine;
using RPG.Control;
using RPG.Combat;

namespace RPG.Core {
    public class AIController : MonoBehaviour {
        [SerializeField] private EnemySO enemy;
        private PlayerController player;
        private Vector2 startPosition;
        EnemyState activeState;

        public EnemyState ActiveState {
            get => activeState;
            set => activeState = value;
        }

        public PlayerController Player {
            get => player;
        }

        public Vector2 StartPosition {
            get => startPosition;
        }

        public enum EnemyState {
            CHASE,
            ATTACK,
            RUN,
            IDLE
        }

        private void Start() {
            player = FindObjectOfType<PlayerController>();
            startPosition = transform.position;
            activeState = EnemyState.IDLE;
        }

        private void Update() {
            float playerDistance = Vector2.Distance(player.transform.position, transform.position);
            if (playerDistance <= enemy.ChaseDistance && playerDistance > enemy.AttackDistance) {
                activeState = EnemyState.CHASE;
            } else if (playerDistance <= enemy.AttackDistance && playerDistance > enemy.RepelDistance) {
                activeState = EnemyState.ATTACK;
            } else if (playerDistance <= enemy.RepelDistance) {
                activeState = EnemyState.RUN;
            } else {
                activeState = EnemyState.IDLE;
            }
        }

        private void OnDrawGizmos() {
            var position = transform.position;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, enemy.ChaseDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, enemy.AttackDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(position, enemy.RepelDistance);
        }
    }
}