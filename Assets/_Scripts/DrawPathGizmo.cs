using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawPathGizmo : MonoBehaviour
{
    [SerializeField] Color LineColor = Color.green;
    [SerializeField] Color PointsColor = Color.red;

    public Transform transformRootObject;
    WaypointNode[] waypointNodes;

    private void Awake()
    {
        //for (int i = 0; i < transformRootObject.childCount; i++)
        //{
        //    WaypointNode w = transformRootObject.GetChild(i).GetComponent<WaypointNode>();
        //    int p = i + 1;
        //    if(p >= transformRootObject.childCount)
        //    {
        //        p = 0;
        //    }
        //    w.nextWaypointNode[0] = transformRootObject.GetChild(p).GetComponent<WaypointNode>();
        //}
    }


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
