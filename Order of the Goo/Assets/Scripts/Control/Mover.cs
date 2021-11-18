using System.Collections;
using Pathfinding;
using UnityEngine;

public class Mover : MonoBehaviour {
    [SerializeField] float moveSpeed;
    [SerializeField] private MoverState currentState = MoverState.NORMAL;
    private Rigidbody2D rb;
    private Vector2 velocity;
    private AIPath path;
    private float startTime;
    private bool shouldRepeatPath;
    private GameObject destinationObject;
    private float delay;

    private enum MoverState {
        ASTAR,
        NORMAL
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        path = GetComponent<AIPath>();
        shouldRepeatPath = false;
        startTime = Time.time; 
        if (path != null)
            StopMoving(); 
    }

    private void Update() {
        if (currentState == MoverState.NORMAL) {
            UpdateVelocity();
        } else {
            if (shouldRepeatPath) {
                UpdatePath();
            }
        }
    }

    private void UpdateVelocity() {
        rb.velocity = velocity;
    }

    public void SetDirection(Vector2 direction) {
        velocity = direction.normalized * moveSpeed;
    }

    public void StopMoving() { 
        velocity = Vector2.zero;
    }

    public void MoveUsingAStar(Vector2 destination) {  
        path.destination = destination; 
    }

    public void StartAStar() {     
        path.canMove = true;
    }

    public void SetUpdatePath(GameObject destinationObject, float delay) {
        this.destinationObject = destinationObject;
        this.delay = delay;
        shouldRepeatPath = true;
    }

    private void UpdatePath() {
        bool delayFulfilled = Time.time - startTime >= delay;
        if (delayFulfilled) {
            MoveUsingAStar(destinationObject.transform.position);
            startTime = Time.time;
        }
    }

    public void StopAStar() {
        shouldRepeatPath = false;
        path.canMove = false;
    }
}