using UnityEngine;
using RPG.Combat;

public class Kingu : Enemy {
    private Projectile slimeGlobPrefab;
    private AttackState currentAttackState = AttackState.CHARGE;
    public enum AttackState {
        CHARGE,
        SLIME_LAUNCHER,
        SLICE
    }

    public Projectile SlimeGlobPrefab {
        get => slimeGlobPrefab;
    }

    public AttackState CurrentAttackState {
        get => currentAttackState;
        set => currentAttackState = value;
    } 

    public override void Start() {
        base.Start();
        CurrentState = EnemyState.ATTACK;
        RigidbodyMovement.Stop();
        Health.OnHit.AddListener(ProcessHealth);
    }

    private void ProcessHealth() {
        var normalizedHealth = Normalize(0, Health.MaxHealth, Health.TotalHealth);
        if (normalizedHealth <= 0.67f) {
            currentAttackState = AttackState.SLIME_LAUNCHER;
        } else if (normalizedHealth <= 0.33f) {
            currentAttackState = AttackState.SLICE;
        }
    }


    public override void Update() {
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
        RigidbodyMovement.FixedUpdate();
    }

    private float Normalize(float min, float max, float value) {
        return Mathf.InverseLerp(min, max, value);
    }
}
