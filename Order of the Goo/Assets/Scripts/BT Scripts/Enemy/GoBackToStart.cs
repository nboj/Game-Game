using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime; 
using UnityEngine;

public class GoBackToStart : Action {
    private AStarMovement aStarMovement;
    private Enemy enemy;
    public override void OnStart() {
        enemy = GetComponent<Enemy>();
        aStarMovement = enemy.ASMovement;
        aStarMovement.StartAStar();
    }

    public override TaskStatus OnUpdate() {
        if (enemy.CurrentState == Enemy.EnemyState.RETURN) {
            aStarMovement.MoveUsingAStar(enemy.StartPos);
            return TaskStatus.Running;
        }
        aStarMovement.StopAStar();
        return TaskStatus.Failure; 
    }
}