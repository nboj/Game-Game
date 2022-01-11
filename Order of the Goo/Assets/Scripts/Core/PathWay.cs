using UnityEngine; 
using System.Collections.Generic;
using UnityEngine.Events;

public class PathWay : MonoBehaviour {
    [SerializeField] private bool repeat = true;
    [SerializeField] private UnityEvent onReachedEnd;
    List<Transform> waypoints;
    int index;
    public void Awake() {
        waypoints = new List<Transform>();
        foreach (Transform t in transform) { 
            waypoints.Add(t);
        }
        index = 0;
    }

    public Transform GetNextWaypoint() {
        if (index >= waypoints.Count) {
            if (repeat) { 
                index = 0;
            } else {
                index--;
                onReachedEnd.Invoke();
            }
        }
        return waypoints[index++];
    }

}
