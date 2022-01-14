using UnityEngine;
 
public class Movement {
    protected bool canControl = true; 
    private float movementSpeed;
    private Vector2 direction;
    private Animator animator;
    private MovementState currentMovementState;

    public bool CanControl {
        get => canControl;
        set {
            Direction = Vector2.zero;
            if (!value) {
                animator.enabled = false;
            } else {
                animator.enabled = true;
            }
            canControl = value;
        }
    }

    public float MovementSpeed {
        get => movementSpeed;
        set => movementSpeed = value;
    }

    public Vector2 Direction { 
        get => direction;
        set { direction = value; }
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
        if (!canControl) { 
            return;
        }
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
        if (direction == Vector2.zero) {
            ResetAnimator();
            switch (currentMovementState) {
                case MovementState.DOWN:
                    animator.SetBool("Idle Down", true);
                    break;
                case MovementState.LEFT:
                    animator.SetBool("Idle Left", true);
                    break;
                case MovementState.RIGHT:
                    animator.SetBool("Idle Right", true);
                    break;
                default:
                    animator.SetBool("Idle Up", true);
                    break;
            }
        }
    } 

    private void ResetAnimator() {
        animator.SetBool("Right", false);
        animator.SetBool("Left", false);
        animator.SetBool("Up", false);
        animator.SetBool("Down", false);
        animator.SetBool("Idle Down", false);
        animator.SetBool("Idle Up", false);
        animator.SetBool("Idle Right", false);
        animator.SetBool("Idle Left", false);
    }

    protected void StopMoving() { 
        direction = Vector2.zero;
    }
} 