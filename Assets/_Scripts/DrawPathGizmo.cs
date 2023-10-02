using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawPathGizmo : MonoBehaviour
{
    [SerializeField] Color GizmoColor = Color.green;

    public Transform transformRootObject;
    WaypointNode[] waypointNodes;

    void OnDrawGizmos()
    {
        Gizmos.color = GizmoColor;

        if (transformRootObject == null)
            return;

        waypointNodes = transformRootObject.GetComponentsInChildren<WaypointNode>();

        foreach (WaypointNode waypoint in waypointNodes)
        {

            foreach (WaypointNode nextWayPoint in waypoint.nextWaypointNode)
            {
                if (nextWayPoint != null)
                    Gizmos.DrawLine(waypoint.transform.position, nextWayPoint.transform.position);

            }

        }
    }

}
