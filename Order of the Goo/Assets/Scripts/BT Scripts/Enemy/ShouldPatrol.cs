using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using RPG.Core;

public class ShouldPatrol : Conditional {
    [SerializeField] private GameObject player;
    private AIController aiController;
    
    public override void OnStart() {
        
    }

    public override TaskStatus OnUpdate() {
        if (aiController.ActiveState == AIController.EnemyState.IDLE) {
            return TaskStatus.Success;
        } else {
            return TaskStatus.Failure;
        }
    }
}