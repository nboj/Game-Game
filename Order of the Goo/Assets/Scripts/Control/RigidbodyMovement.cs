using UnityEngine;

public class RigidbodyMovement : Movement {
    private Rigidbody2D rigidbody; 

    public RigidbodyMovement(float speed, Rigidbody2D rigidbody) : base(speed) {
        this.rigidbody = rigidbody;
        Direction = Vector2.zero;
    } 

    public void FixedUpdate() {
        rigidbody.velocity = Direction * MovementSpeed;
    }
}