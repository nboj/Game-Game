using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using RPG.Core; 
using UnityEngine;

public class ShouldMove : Conditional {
    public SharedGameObject player;
    private AIController ai;

    public override void OnStart() {
        ai = GetComponent<AIController>();   
    }

    public override TaskStatus OnUpdate()
    {  
        if (ai.ActiveState == AIController.EnemyState.CHASE)
            return TaskStatus.Success;
        else 
            return TaskStatus.Failure;
    }
}
