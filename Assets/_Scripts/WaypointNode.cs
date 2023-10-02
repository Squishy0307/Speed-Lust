using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNode : MonoBehaviour
{
    public float minDistanceToReachWaypoint = 7f;
    public float stoppingDistance = 5f;
    public WaypointNode[] nextWaypointNode;
}