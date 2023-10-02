using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShipAI : MonoBehaviour
{
    private ShipController ship;


    private Vector3 targetPosition;

    WaypointNode currentWaypoint;
    WaypointNode[] allWaypoints;
  
    void Awake()
    {
        ship = GetComponent<ShipController>();
        allWaypoints = FindObjectsOfType<WaypointNode>(); 
    }


    void Update()
    {
        if (currentWaypoint == null)
        {
            currentWaypoint = FindClosestWaypoint();
        }

        if (currentWaypoint != null)
        {
            SetTargetPosition(currentWaypoint.transform.position);
        }

        float forwardAmount = 0f;
        float turnAmount = 0f;

        float reachedTargetDistance = currentWaypoint.minDistanceToReachWaypoint;
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if(distanceToTarget > reachedTargetDistance)
        {
            //Still Too Far, keep going

            //Moving Forward Stuff
            Vector3 dirToMove = (targetPosition - transform.position).normalized;
            //if above 0 then target is in front else target is behind
            float dot = Vector3.Dot(ship.GetShipTransform().forward, dirToMove);

            if (dot > 0)
            {
                //Target in Front
                forwardAmount = 1f;

                float stoppingDistance = currentWaypoint.stoppingDistance; //ADD THIS in node so we can manually change slowing speed for each node
                float stoppingSpeed = 50f;

                if(distanceToTarget < stoppingDistance && ship.GetCurrentSpeed() > stoppingSpeed)
                {
                    //Witting stopping distance and moving forward too fast
                    forwardAmount = -1f;
                }

            }
            else
            {
                float reverseDistance = currentWaypoint.minDistanceToReachWaypoint;//25f;

                currentWaypoint = currentWaypoint.nextWaypointNode[Random.Range(0,currentWaypoint.nextWaypointNode.Length)];

                //if (distanceToTarget > reverseDistance)
                //{
                //    //Too Far to Reverse
                //    forwardAmount = 1f;
                //}
                //else
                //{
                //    forwardAmount = -1f;
                //}
            }

            //Turning ship
            //gets an angle to make the ship turn (value is positive if target is to right and negative if target is on left)
            float angleToDirection = Vector3.SignedAngle(ship.GetShipTransform().forward, dirToMove, Vector3.up);

            if (angleToDirection > 0)
            {
                turnAmount = 1f;
            }
            else
            {
                turnAmount = -1f;
            }
        }
        else
        {
            //TragetReached
            if (ship.GetCurrentSpeed() > 15f)
            {
                forwardAmount = -1f;
            }
            else
            {
                forwardAmount = 0;
                turnAmount = 0;
            }
        }

        if ( ship != null )
        {
            ship.accel = forwardAmount;
            ship.horz = turnAmount;
        }
        else
        {
            Debug.LogError("Add Ship Controller Component to this object!");
        }
    }

    WaypointNode FindClosestWaypoint()
    {
        return allWaypoints.OrderBy(t => Vector3.Distance(transform.position,t.transform.position)).FirstOrDefault();
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
