using UnityEngine;

public class Mover : MonoBehaviour {
    [SerializeField] float moveSpeed;
    private Rigidbody2D rb;
    private Vector2 velocity;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        StopMoving();
    }

    private void Update() {
        rb.velocity = velocity;
    }

    public void SetDirection(Vector2 direction) {
        velocity = direction.normalized * moveSpeed;
    }

    public void StopMoving() { 
        velocity = Vector2.zero;
    }
}