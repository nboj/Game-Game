using UnityEngine;
 
public class Movement {
    private float movementSpeed;
    private Vector2 direction;
    private MovementState lastMovementState;

    public float MovementSpeed => movementSpeed;

    public Vector2 Direction {
        get => direction;
        set => direction = value;
    }
    
    private enum MovementState {
        UP,
        RIGHT,
        LEFT,
        DOWN
    }

    public Movement(float speed) {
        movementSpeed = speed;
        direction = Vector2.zero;
        lastMovementState = MovementState.DOWN;
    } 
    
    public void SetDirection(Vector2 direction) {
        this.direction = direction.normalized;
    }

    protected void StopMoving() { 
        direction = Vector2.zero;
    }
} 