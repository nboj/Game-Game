using BehaviorDesigner.Runtime.Tasks; 
using RPG.Combat;
using UnityEngine;
using RPG.Core;

public class Attack : Action {
    private Fighter fighter;
    private AIController ai;
    private Mover mover;
    public override void OnStart() {
        fighter = GetComponent<Fighter>();
        ai = GetComponent<AIController>();
        mover = GetComponent<Mover>();
    }

    public override TaskStatus OnUpdate()
    { 
        fighter.FireWeapon(ai.Player.transform.position, ai.SelectedIndex);
        mover.StopMoving();
        if (ai.ActiveState != AIController.EnemyState.ATTACK)
            return TaskStatus.Success;
        else
            return TaskStatus.Running;
    }
}