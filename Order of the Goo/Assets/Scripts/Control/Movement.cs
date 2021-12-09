using UnityEngine;
 
public class Movement {
    private float movementSpeed;
    private Vector2 direction;
    private Animator animator;
    private MovementState currentMovementState;

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

    public Movement(float speed, Animator animator) {
        this.animator = animator;
        movementSpeed = speed;
        direction = Vector2.zero;
        currentMovementState = MovementState.DOWN;
    } 
    
    public void SetDirection(Vector2 direction) {
        this.direction = direction.normalized;
    } 

    public void SetAnimator(Vector2 direction) {
        ResetAnimator();
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            if (direction.x > 0) {
                animator.SetBool("Right", true);
                currentMovementState = MovementState.RIGHT;
            } else if (direction.x < 0) {
                animator.SetBool("Left", true);
                currentMovementState = MovementState.LEFT;
            }
        } else {
            if (direction.y > 0) {
                animator.SetBool("Up", true);
                currentMovementState = MovementState.UP;
            } else if (direction.y < 0) {
                animator.SetBool("Down", true);
                currentMovementState = MovementState.DOWN;
            } 
        }
    }

    private void ResetAnimator() {
        animator.SetBool("Right", false);
        animator.SetBool("Left", false);
        animator.SetBool("Up", false);
        animator.SetBool("Down", false);
    }

    protected void StopMoving() { 
        direction = Vector2.zero;
    }
} 