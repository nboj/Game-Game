using UnityEngine;

public class RigidbodyMovement : Movement {
    private Rigidbody2D rigidbody;

    public RigidbodyMovement(float speed, Rigidbody2D rigidbody, Animator animator) : base(speed, animator) {
        this.rigidbody = rigidbody;
        Direction = Vector2.zero;
    } 

    public void FixedUpdate() {
        if (canControl)
            rigidbody.velocity = Direction * MovementSpeed;
        else {
            rigidbody.velocity = Vector2.zero;
        }
    }
}