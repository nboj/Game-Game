using System.Collections;
using Pathfinding;
using UnityEngine;

public class Mover : MonoBehaviour {
    [SerializeField] float moveSpeed;
    private Rigidbody2D rb;
    private Vector2 velocity;
    private MoverState currentState;
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
        if (rb == null) {
            currentState = MoverState.ASTAR;
            startTime = Time.time;

        } else {
            currentState = MoverState.NORMAL;
            StopMoving();
        } 
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
            Debug.Log(destinationObject.transform.position);
            MoveUsingAStar(destinationObject.transform.position);
            startTime = Time.time;
        }
    }

    public void StopAStar() {
        shouldRepeatPath = false;
        path.canMove = false;
    }
}