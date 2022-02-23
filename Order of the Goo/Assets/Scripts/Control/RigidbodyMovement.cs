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
            TranslateUsingPhysics(Direction * MovementSpeed);
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

    public void TranslateUsingPhysics(Vector2 positionTranslation) {
        rigidbody.MovePosition(rigidbody.position + positionTranslation * Time.deltaTime);
    }

    public void TranslateUsingPhysicsRaw(Vector2 positionTranslation) {
        rigidbody.MovePosition(positionTranslation);
    }

    public void AddForce(Vector2 force) {
        rigidbody.AddForce(force);
    }
}