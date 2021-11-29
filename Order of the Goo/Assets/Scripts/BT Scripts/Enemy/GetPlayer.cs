using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine; 

public class GetPlayer : Action { 
    public SharedGameObject player;  
    public override void OnStart() {
        player.Value = GameObject.FindObjectOfType<Player>().gameObject; 
    }
    public override TaskStatus OnUpdate() { 
        if (player == null)
            return TaskStatus.Failure; 
        return TaskStatus.Success;
    }
}
