using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNode : MonoBehaviour
{
    public float minDistanceToReachWaypoint = 7f;
    public bool SlowDownOnThisPoint = false;
    public float stoppingDistance = 5f;
    public float stoppingSpeed = 80f;
    public WaypointNode[] nextWaypointNode;
    public bool RandomPos = true;

    public Vector3 getPosition()
    {
        if (RandomPos)
        {
            Vector3 minBound = transform.position + transform.right * 9f;
            Vector3 maxBound = transform.position - transform.forward * 9f;

            return Vector3.Lerp(minBound, maxBound, Random.Range(0, 2));
        }
        else 
        { 
            return transform.position; 
        }
        //return transform.position + Random.insideUnitSphere * 6.5f;
    }
}