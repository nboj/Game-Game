using UnityEngine;
using UnityEngine.Events;

public class FollowPath : MonoBehaviour {
    [SerializeField] Path path;
    [SerializeField] private bool useStartTrigger;
    [SerializeField] private UnityEvent startTrigger;
    private Creature creature;
    private Transform currentWaypoint;
    private void Start() {
        path.enabled = false;
        if (!useStartTrigger) {
            Setup();
        } else {
            startTrigger.AddListener(Setup);
        }
    }

    private void Setup() {
        path.enabled = true;
        creature = GetComponent<Creature>();
        creature.ASMovement.StartAStar();
        MoveAlongPath();
        Debug.Log(currentWaypoint);
        Debug.Log(creature);
    }

    private void Update() {
        if (path != null && path.isActiveAndEnabled) {
            creature.RigidbodyMovement.SetAnimator(creature.ASMovement.Direction);
            if (Vector3.Distance(currentWaypoint.position, transform.position) <= 0.5f) {
                MoveAlongPath();
            }
        }
    }

    private void MoveAlongPath() {
        var nextWaypoint = path.GetNextWaypoint();
        currentWaypoint = nextWaypoint;
        creature.ASMovement.MoveUsingAStar(currentWaypoint.position);
    }

    public void StartPath() {
        startTrigger.Invoke();  
    }
}