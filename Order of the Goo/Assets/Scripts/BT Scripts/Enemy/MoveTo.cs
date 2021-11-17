using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using RPG.Control;
using UnityEngine;
using RPG.Core;

public class MoveTo : Action {   
    public SharedGameObject player;
    private Mover mover;
    private AIController ai; 
    
    public override void OnStart() {
        mover = GetComponent<Mover>();    
        ai = GetComponent<AIController>(); 
    }
    public override TaskStatus OnUpdate() {
        mover.SetDirection(player.Value.transform.position - mover.transform.position);
        if (ai.ActiveState != AIController.EnemyState.CHASE)
            return TaskStatus.Success;
        else { 
            return TaskStatus.Running;
        }
    }
}
