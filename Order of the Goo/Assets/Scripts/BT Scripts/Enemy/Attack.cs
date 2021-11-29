using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Attack : Action {
    public SharedGameObject player;

    private Fighter fighter;
    private Enemy enemy;

    public override void OnStart() {
        fighter = GetComponent<Fighter>();
        enemy = GetComponent<Enemy>();
    }

    public override TaskStatus OnUpdate() {
        if (enemy.CurrentState != Enemy.EnemyState.ATTACK)
            return TaskStatus.Failure;
        enemy.Fire(player.Value.transform.position);
        return TaskStatus.Running;
    }
}