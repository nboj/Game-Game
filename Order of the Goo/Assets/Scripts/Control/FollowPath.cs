using UnityEngine;

public class FollowPath : MonoBehaviour {
    [SerializeField] Path path;
    private Creature creature;
    private Transform currentWaypoint;
    private void Start() {
        creature = GetComponent<Creature>();
        creature.ASMovement.StartAStar();
        MoveAlongPath();
        Debug.Log(currentWaypoint);
        Debug.Log(creature);
    }

    private void Update() {
        creature.RigidbodyMovement.SetAnimator(creature.ASMovement.Direction);
        if (Vector3.Distance(currentWaypoint.position, transform.position) <= 0.5f) {
            MoveAlongPath();
        }
    }

    private void MoveAlongPath() {
        var nextWaypoint = path.GetNextWaypoint();
        currentWaypoint = nextWaypoint;
        creature.ASMovement.MoveUsingAStar(currentWaypoint.position);
    }
}