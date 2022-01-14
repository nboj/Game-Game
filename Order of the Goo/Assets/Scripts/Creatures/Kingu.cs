using UnityEngine;
using RPG.Combat;

public class Kingu : Enemy {
    private Projectile slimeGlobPrefab;
    private AttackState currentAttackState = AttackState.SLIME_LAUNCHER;
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
        RigidbodyMovement.Stop();
    }


    public override void Update() {
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
        RigidbodyMovement.FixedUpdate();
    }
}
