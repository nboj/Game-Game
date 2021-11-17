using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using RPG.Core; 
using UnityEngine;

public class ShouldAttack : Conditional {
    public SharedGameObject player;
    private AIController ai;
    private Mover mover;

    public override void OnStart() {
        ai = GetComponent<AIController>();
        mover = GetComponent<Mover>();
    }

    public override TaskStatus OnUpdate()
    {
        if (ai.ActiveState == AIController.EnemyState.ATTACK) 
            return TaskStatus.Success; 
        return TaskStatus.Failure;
    }
}
