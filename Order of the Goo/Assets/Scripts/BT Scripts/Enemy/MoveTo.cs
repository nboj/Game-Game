using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;  
using UnityEngine;

public class MoveTo : Action {
    [SerializeField] private float pathfinderDelay = 0.5f;
    public SharedGameObject player;
    private AStarMovement aStarMovement;
    private Enemy enemy;
    private float startTime;

    public override void OnStart() {
        enemy = GetComponent<Enemy>();
        aStarMovement = enemy.ASMovement; 
        aStarMovement.SetUpdatePath(player.Value, pathfinderDelay); 
        aStarMovement.StartAStar();
    }

    public override TaskStatus OnUpdate() { 
        if (enemy.CurrentState != Enemy.EnemyState.CHASE) {
            aStarMovement.StopAStar();
            return TaskStatus.Failure;
        }
        aStarMovement.UpdatePath();
        return TaskStatus.Running; 
    } 
}