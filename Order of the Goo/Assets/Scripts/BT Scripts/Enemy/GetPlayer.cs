using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine;
using RPG.Control;

public class GetPlayer : Action { 
    public SharedGameObject player;  
    public override void OnStart() {
        player.Value = GameObject.FindObjectOfType<PlayerController>().gameObject; 
    }
    public override TaskStatus OnUpdate() { 
        if (player == null)
            return TaskStatus.Failure; 
        return TaskStatus.Success;
    }
}
