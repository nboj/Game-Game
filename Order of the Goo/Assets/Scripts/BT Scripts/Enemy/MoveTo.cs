using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime; 
using RPG.Core;
using UnityEngine;

public class MoveTo : Action {
    [SerializeField] private float pathfinderDelay = 0.5f;
    public SharedGameObject player;
    private Mover mover;
    private AIController ai;
    private float startTime;

    public override void OnStart() {
        mover = GetComponent<Mover>();
        ai = GetComponent<AIController>();
        mover.SetUpdatePath(player.Value, pathfinderDelay); 
        mover.StartAStar();
    }

    public override TaskStatus OnUpdate() { 
        if (ai.ActiveState != AIController.EnemyState.CHASE) {
            mover.StopAStar();
            return TaskStatus.Success;
        } else {
            return TaskStatus.Running;
        }
    } 
}