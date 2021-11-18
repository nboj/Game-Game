using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using RPG.Core;
using UnityEngine;

public class GoBackToStart : Action {
    [SerializeField] private SharedGameObject player;
    private Mover mover;
    private AIController ai;
    public override void OnStart() {
        mover = GetComponent<Mover>();
        ai = GetComponent<AIController>();
        mover.StartAStar();
    }

    public override TaskStatus OnUpdate() {
        if (ai.ActiveState == AIController.EnemyState.IDLE) {
            mover.MoveUsingAStar(ai.StartPosition);
            return TaskStatus.Running;
        } else {
            mover.StopAStar();
            return TaskStatus.Success;
        }
    }
}