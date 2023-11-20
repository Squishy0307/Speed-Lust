using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShipAI : MonoBehaviour
{
    private VehicleMovement ship;
    private Vector3 targetPosition;


    WaypointNode currentWaypoint;
    WaypointNode[] allWaypoints;

    [SerializeField] float shipDetectionRadius = 10f;
    [SerializeField] float rayMaxDistance = 6.08f;
    [SerializeField] LayerMask shipLayer;
    [SerializeField] LayerMask wallLayer;

    private Vector3 rayOrigin;
    private Vector3 rayDirection;

    private Vector3 currentTargetPos;
    float forwardAmount = 0f;

    //Avoidance
    Vector3 avoidanceVectorLerped = Vector3.zero;

    private float thruster;
    private float rudder;
    private bool isBraking;
    private float lastDirection;

    void Awake()
    {
        ship = GetComponent<VehicleMovement>();
        allWaypoints = FindObjectsOfType<WaypointNode>(); 
    }

    private void Start()
    {
        FindSetNextTargetPos();
    }


    void Update()
    {
        //TO DO: Add respawn is the ship velosity stays almost same for more than few seconds 
        //       Control the pitch of the ship based on ground normal
    
        float turnAmount = 0f;
       

        float reachedTargetDistance = currentWaypoint.minDistanceToReachWaypoint;
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        //Debug.Log(distanceToTarget);

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
                float stoppingSpeed = currentWaypoint.stoppingSpeed; //50

                if(distanceToTarget < stoppingDistance && ship.GetCurrentSpeed() > stoppingSpeed)
                {
                    //Witting stopping distance and moving forward too fast
                    //forwardAmount = -1f;
                    forwardAmount = 0.5f;
                }

            }
            else
            {
                currentWaypoint = currentWaypoint.nextWaypointNode[Random.Range(0,currentWaypoint.nextWaypointNode.Length)];
                FindSetNextTargetPos();
            }

            AvoidAiShips(dirToMove,out dirToMove);

            //Turning ship
            //gets an angle to make the ship turn (value is positive if target is to right and negative if target is on left)
            float angleToDirection = Vector3.SignedAngle(ship.GetShipTransform().forward, dirToMove, ship.GetShipTransform().up);
            lastDirection = Mathf.Lerp(lastDirection,angleToDirection,Time.deltaTime * 4.5f);
            //Debug.Log(angleToDirection);

            if (lastDirection > 2)
            {
                turnAmount = 1;
            }
            else if (lastDirection < -2)
            {
                turnAmount = -1;
            }

            else
            {
                turnAmount = 0;
            }
        }
        else
        {
            //TragetReached
            if (ship.GetCurrentSpeed() > currentWaypoint.stoppingSpeed) //75
            {
                forwardAmount = -0.2f;
                Debug.Log("reached");
            }

            else
            {
                Debug.Log("reached 0");
                forwardAmount = 0;
                turnAmount = 0;

                currentWaypoint = currentWaypoint.nextWaypointNode[Random.Range(0, currentWaypoint.nextWaypointNode.Length)];
                FindSetNextTargetPos();
            }
        }

        if ( ship != null )
        {
            thruster = forwardAmount; //applyThrottleOrBrake(turnAmount);
            rudder = turnAmount;

            ship.SetInputs(rudder, thruster, false);
        }
        else
        {
            Debug.LogError("Add Ship Controller Component to this object!");
        }
    }

    void FindSetNextTargetPos()
    {
        if (currentWaypoint == null)
        {
            currentWaypoint = FindClosestWaypoint();
        }

        if (currentWaypoint != null)
        {
            currentTargetPos = currentWaypoint.getPosition();
            SetTargetPosition(currentTargetPos);
        }

    }

    float applyThrottleOrBrake(float input)
    {
        return 1.05f - Mathf.Abs(input) / 1f;
    }

    public void recalculateNeareastWaypoint()
    {
        currentWaypoint = FindClosestWaypoint();
    }

    WaypointNode FindClosestWaypoint()
    {
        return allWaypoints.OrderBy(t => Vector3.Distance(transform.position,t.transform.position)).FirstOrDefault();
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    bool IsShipInFrontOfThisAI(out Vector3 position, out Vector3 otherShipRightVector, out Vector3 otherShipLeftVector)
    {
        RaycastHit hit;
        RaycastHit wallCheckRay;

        rayOrigin = transform.position;
        rayDirection = ship.GetShipTransform().forward;


        //cast a sphere in front of ship and check if there is another ship in front
        if (Physics.SphereCast(rayOrigin, shipDetectionRadius, rayDirection, out hit, rayMaxDistance, shipLayer, QueryTriggerInteraction.UseGlobal))
        {
            Debug.DrawRay(rayOrigin, rayDirection * rayMaxDistance, Color.red);

            position = hit.collider.transform.position;
            otherShipRightVector = hit.collider.transform.right;
            otherShipLeftVector = hit.collider.transform.right * -1f;

            return true;
        }


        if (Physics.Raycast(rayOrigin, rayDirection, out wallCheckRay, rayMaxDistance + 5f, wallLayer))
        {

            position = wallCheckRay.point;
            otherShipRightVector = wallCheckRay.normal;
            otherShipLeftVector = wallCheckRay.normal;

            //otherShipLeftVector.Normalize();
            //otherShipRightVector.Normalize();

            return true;

        }


        position = Vector3.zero;
        otherShipRightVector = Vector3.zero;
        otherShipLeftVector = Vector3.zero;

        return false;
    }

    void AvoidAiShips(Vector3 vectorToTarget, out Vector3 newVectorToTarget)
    {
        if(IsShipInFrontOfThisAI(out Vector3 otherShipPosition, out Vector3 otherShipRightVector, out Vector3 otherShipLeftVector))
        {
            Vector3 avoidanceVector = Vector3.zero;

            if (Vector3.Dot(ship.GetShipTransform().forward, vectorToTarget) > 0)
            {
                avoidanceVector = Vector3.Reflect((otherShipPosition - transform.position).normalized, otherShipRightVector);
                forwardAmount = 0f;
            }
            else
            {
                avoidanceVector = Vector3.Reflect((otherShipPosition - transform.position).normalized, otherShipLeftVector);
            }

            float distanceToTarget = (targetPosition - transform.position).magnitude;

            //We want to be able to control how much desire the AI has to drive towards the waypoint vs avoid the other ships.
            //As we get closer to the waypoint the desire to reach waypoint increases.
            float driveToTargetInfluence = 6 / distanceToTarget;
            
            //Ensure that we limit the value to between 30% and 100% as we always want the AI to desire to reach the waypoint.
            driveToTargetInfluence = Mathf.Clamp(driveToTargetInfluence, 0.30f, 1.0f);

            //the desire to avoid the car is simply the inverse to reach the waypoint
            float avoidanceInfluence = 1 - driveToTargetInfluence;

            //reduce jittering a little bit by using lerp
            avoidanceVectorLerped = Vector3.Lerp(avoidanceVectorLerped, avoidanceVector, Time.fixedDeltaTime * 5);

            newVectorToTarget = vectorToTarget * driveToTargetInfluence + avoidanceVectorLerped * avoidanceInfluence;
            newVectorToTarget.Normalize();

            //draw the avoidance vector
            Debug.DrawRay(transform.position, avoidanceVector * 10, Color.green);
            //draw the vector that the car will take
            Debug.DrawRay(transform.position, newVectorToTarget * 10, Color.yellow);

            return;
        }

        newVectorToTarget = vectorToTarget;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * rayMaxDistance);
        Gizmos.DrawWireSphere(rayOrigin + rayDirection * rayMaxDistance, shipDetectionRadius);

        //Gizmos.DrawSphere(currentTargetPos, 1f);
    }

}
