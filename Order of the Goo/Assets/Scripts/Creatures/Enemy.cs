using System;
using Pathfinding;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(AIPath))]
public class Enemy : AggressiveCreature {
    [SerializeField] private Enemy_SO enemy;
    private EnemyState currentState;
    private Player player;
    private Vector2 startPos;

    public EnemyState CurrentState => currentState;
    public Vector2 StartPos => startPos;

    public override void Start() {
        base.Start();
        player = FindObjectOfType<Player>();
        currentState = EnemyState.IDLE;
        startPos = transform.position;
    }

    public enum EnemyState {
        CHASE,
        ATTACK,
        RETURN,
        IDLE
    }

    public override void Update() {
        var playerDistance = Vector2.Distance(player.transform.position, transform.position);
        if (playerDistance <= enemy.AttackDistance) {
            currentState = EnemyState.ATTACK;
        } else if (playerDistance > enemy.AttackDistance && playerDistance <= enemy.ChaseDistance) {
            currentState = EnemyState.CHASE;
        } else if (playerDistance > enemy.ChaseDistance) {
            currentState = EnemyState.RETURN;
        } else {
            currentState = EnemyState.IDLE;
        }
    }

    private void OnDrawGizmos() {
        var position = transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position, enemy.ChaseDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, enemy.AttackDistance);
    }
} 