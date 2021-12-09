using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(AIPath))]
public class AStarMovement : Movement { 
    private AIPath path;
    private float startTime; 
    private GameObject destinationObject;
    private float delay;
    private Creature creature;
    
    public AStarMovement(float speed, AIPath path, Animator animator, Creature creature) : base(speed, animator) {
        this.creature = creature;
        startTime = Time.time;
        this.path = path;
        if (path != null) {
            StopMoving();
            path.maxSpeed = speed;
        }
    }
    
    public void MoveUsingAStar(Vector2 destination) { 
        SetDirection((destination - (Vector2)creature.transform.position).normalized);
        path.destination = destination;  
    }

    public void StartAStar() {     
        path.canMove = true;
    }

    public void SetUpdatePath(GameObject destinationObject, float delay) {
        this.destinationObject = destinationObject;
        this.delay = delay; 
    }

    public void UpdatePath() { 
        bool delayFulfilled = Time.time - startTime >= delay;
        if (delayFulfilled) { 
            MoveUsingAStar(destinationObject.transform.position);
            startTime = Time.time;
        }
    }

    public void StopAStar() { 
        path.canMove = false;
    }
} 