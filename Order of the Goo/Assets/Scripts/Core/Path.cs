using UnityEngine; 
using System.Collections.Generic;

public class Path : MonoBehaviour {
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
            index = 0;
        }
        return waypoints[index++];
    }

}
