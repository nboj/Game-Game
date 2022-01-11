using System;
using BehaviorDesigner.Runtime;
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
        CreatureSO = enemy;
        player = FindObjectOfType<Player>();
        currentState = EnemyState.IDLE;
        startPos = transform.position;
        Health.OnDeath.AddListener(HandleDeath);
        Health.OnRestore.AddListener(HandleRestore);
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

    public void HandleDeath() {
        Health.HideDisplay();
        Animator.enabled = false;
        Health.enabled = false;
        GetComponent<Enemy>().enabled = false;
        GetComponent<BehaviorTree>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().color = Color.black;
        GetComponent<Rigidbody2D>().simulated = false;
    }

    public void HandleRestore() {
        Health.ShowDisplay();
        Animator.enabled = true;
        Health.enabled = true;
        GetComponent<Enemy>().enabled = true;
        GetComponent<BehaviorTree>().enabled = true;
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
        GetComponent<Rigidbody2D>().simulated = true;
    }
} 