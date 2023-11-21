using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNode : MonoBehaviour
{
    public float minDistanceToReachWaypoint = 7f;
    public float stoppingDistance = 5f;
    public float stoppingSpeed = 80f;
    public WaypointNode[] nextWaypointNode;

    public Vector3 getPosition()
    {
        //Vector3 minBound = transform.position + transform.right * 5f;
        //Vector3 maxBound = transform.position - transform.forward * 5f;

        //return Vector3.Lerp(minBound, maxBound, Random.Range(0, 2))
        return transform.localPosition + Random.insideUnitSphere * 6.5f;
    }
}