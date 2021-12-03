using RPG.Combat;
using UnityEngine; 
using System.Collections.Generic;

public class MagicProjectile : Projectile { 
    private Health[] foundCreatures;
    private Health targetCreature;
    private MagicWeapon_SO magicWeapon;
    private const float DIVIDER_AMOUNT = 1000f;

    public MagicWeapon_SO MagicWeapon {
        get => magicWeapon;
        set => magicWeapon = value;
    }

    public override void Start() {
        base.Start(); 
    }

    public override void Update() { 
        base.Update();
        if (!canControl)
            return;
        if (magicWeapon.TrackTarget)
            UpdateDirection();
    }

    private void UpdateDirection() {
        if (targetCreature != null && targetCreature.enabled) {
            var targetDirection = (Vector2)(targetCreature.transform.position - transform.position).normalized;
            ProjectileDirection = Vector3.Lerp(ProjectileDirection, targetDirection, magicWeapon.TrackingSensitivity / DIVIDER_AMOUNT);
            SetObjectRotation(ProjectileDirection);
        } else if (targetCreature == null) {
            return;
        } else {
            SetupTracking();
        }
    }

    public void SetupTracking() {
        var shortestDistance = Mathf.Infinity;
        Health targetCreature = null;
        var creatures = FindObjectsOfType<Health>(); 
        for (int i = 0; i < creatures.Length; i++) {
            var distance = Vector2.Distance(creatures[i].transform.position, transform.position);
            var creature = creatures[i];
            if (distance < shortestDistance && !creature.CompareTag(Parent.tag)) {
                shortestDistance = distance;    
                targetCreature = creature;
            }
        }
        foundCreatures = creatures;
        if (targetCreature.enabled && targetCreature != null) {
            this.targetCreature = targetCreature;
        } else if (targetCreature == null) {
            return;
        }
    }

    public override void SetRotation(Vector2 target) {
        base.SetRotation(target); 
    }
}