using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawPathGizmo : MonoBehaviour
{
    [SerializeField] Color LineColor = Color.green;
    [SerializeField] Color PointsColor = Color.red;

    public Transform transformRootObject;
    WaypointNode[] waypointNodes;

    void OnDrawGizmos()
    {      

        if (transformRootObject == null)
            return;

        waypointNodes = transformRootObject.GetComponentsInChildren<WaypointNode>();

        foreach (WaypointNode waypoint in waypointNodes)
        {

            foreach (WaypointNode nextWayPoint in waypoint.nextWaypointNode)
            {
                if (nextWayPoint != null)
                {
                    Gizmos.color = LineColor;
                    Gizmos.DrawLine(waypoint.transform.position, nextWayPoint.transform.position);
                    
                }
                Gizmos.color = PointsColor;
                Gizmos.DrawSphere(waypoint.transform.position, 2f);

            }

        }
    }

}
