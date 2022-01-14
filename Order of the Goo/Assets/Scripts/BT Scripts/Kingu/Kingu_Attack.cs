using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using RPG.Combat;

public class Kingu_Attack : Action {
    private Kingu kingu;
    private Animator animator;
    private Kingu_SO kinguSO;
    private float startAttackDelay; 
    private Player player;
    private AttackingState attackState;
    private Vector2 targetChargeLocation;
    private enum AttackingState {
        ATTACKING,
        IDLE
    }
    public override void OnStart() {
        base.OnStart();
        kingu = GetComponent<Kingu>();
        animator = transform.GetComponentInChildren<Animator>(); 
        kinguSO = (Kingu_SO)kingu.CreatureSO;
        startAttackDelay = Time.time;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        attackState = AttackingState.IDLE;
    }

    public override TaskStatus OnUpdate() {
        base.OnUpdate();
        if (kingu.CurrentState == Enemy.EnemyState.ATTACK) {
            switch (kingu.CurrentAttackState) { 
                case Kingu.AttackState.CHARGE:
                    Charge();
                    break;
                case Kingu.AttackState.SLIME_LAUNCHER:
                    LaunchSlime();
                    break;
                default: // slice
                    Swing();
                    break;
            }
            return TaskStatus.Running;
        } else {
            return TaskStatus.Failure;
        }
    }

    public void Charge() {
        if (Time.time - startAttackDelay >= kinguSO.ChargeDelay && attackState == AttackingState.IDLE) {
            const float EXTRA_DISTANCE_MULTIPLIER = 3f;
            attackState = AttackingState.ATTACKING;
            ResetAnimator();
            animator.SetBool("Attack_1", true); 
            targetChargeLocation = player.transform.position + (player.transform.position - transform.position).normalized * EXTRA_DISTANCE_MULTIPLIER; 
            kingu.RigidbodyMovement.MovementSpeed = kinguSO.ChargeSpeed;
        }
        if (attackState == AttackingState.ATTACKING) { 
            if (Vector2.Distance(targetChargeLocation, transform.position) <= 0.2f) { 
                startAttackDelay = Time.time;
                kingu.RigidbodyMovement.Stop(); 
                attackState = AttackingState.IDLE;
                ResetAnimator();
            } else {
                kingu.RigidbodyMovement.SetDirectionAndAnimation(targetChargeLocation - (Vector2)transform.position); 
            }
        }
    }

    public override void OnCollisionEnter2D(Collision2D other) { 
        base.OnCollisionEnter2D(other);
        if (other.gameObject.CompareTag("Player") && kingu.CurrentAttackState == Kingu.AttackState.CHARGE && attackState == AttackingState.ATTACKING) {
            Debug.Log("Hit");
            var player = other.gameObject.GetComponent<Player>();
            if (player != null) {
                kingu.Fighter.InvokeOnHit(player.gameObject);
            } 
        }
    }

    public void LaunchSlime() { 
        if (Time.time - startAttackDelay >= kinguSO.SlimeLauncherDelay && attackState == AttackingState.IDLE) {
            kingu.RigidbodyMovement.SetDirectionAndAnimation(Vector2.zero);
            ResetAnimator();
            kingu.RigidbodyMovement.SetAnimator(Vector2.down);
            animator.SetBool("Attack_2", true);
            StartCoroutine(SlimeStrike());
            startAttackDelay = Time.time;
        } else if (attackState != AttackingState.IDLE) {
            attackState = AttackingState.IDLE;
        }
    }

    private IEnumerator SlimeStrike() {
        kingu.Fire(Vector2.up * 10);
        yield return new WaitForSeconds(kinguSO.SlimeStrikeDelay);
        targetChargeLocation = player.transform.position;
        kingu.Fire(targetChargeLocation, new Vector3(targetChargeLocation.x, Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight)).y), true);
    }

    public void Swing() { } 

    private void ResetAnimator() {
        animator.SetBool("Attack_1", false);
        animator.SetBool("Attack_2", false);
        animator.SetBool("Attack_3", false); 
    }
}
