using System;
using UnityEngine;

public class RigidbodyMovement : Movement {
    private Rigidbody2D rigidbody;

    public RigidbodyMovement(float speed, Rigidbody2D rigidbody, Animator animator) : base(speed, animator) {
        this.rigidbody = rigidbody;
        Direction = Vector2.zero;
    }

    public void FixedUpdate() {
        if (canControl) {
            rigidbody.MovePosition(rigidbody.position + Direction * MovementSpeed * Time.deltaTime);
            rigidbody.velocity = Vector2.zero;
        }  
    } 

    public void Stop() {
        SetDirection(Vector2.zero); 
    }

    public void SetMovementSpeed(float speed) {
        MovementSpeed = speed;
    }

    internal void SetDirectionAndAnimation(Vector2 dir) {
        SetDirection(dir);
        SetAnimator(dir);
    }
}