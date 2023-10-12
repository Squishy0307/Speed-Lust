using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{

    public int checkpointCount;
    public string lapNumber;

    public Transform nextCheckpoint;
    public Transform previousCheckpoint;

    [SerializeField] private ShipSettings handling;

    [SerializeField] private GameObject shipPrefab;
    [SerializeField] LayerMask groundLayer;

    [Header("Camera Settings")]
    [SerializeField] private float camBackInit = 9.0f; //how much distance you start with
    [SerializeField] private float camBackExtra = 4.0f; //how much more you get at full speed
    [SerializeField] private float camRight = -1.0f;
    [SerializeField] private float camUp = 4.0f;
    [SerializeField] private float camRotate = 0.6f; //how much it looks up/down towards the ship
    [SerializeField] private float camSmoothing = 0.35f;
    [SerializeField] private float camTurnRotation = 0.2f;
    [SerializeField] private GameObject camPrefab;
    [SerializeField] private Transform camSpot;

    private Transform ship, model, camSmooth, camSnappy;
    private Camera cam;
    private GameManager g_manager;
    private Rigidbody rb;

    private Vector3 newGravity = new Vector3(0.0f, -1.0f, 0.0f);
    private float gravityScalar = 9.8f;
    private float vibrate = 0.0f;
    private float currentSpeed, angleChange;
    private int shipID;

    //leaning stuff while moving
    private float prevRotate, prevLean, rotatePercentage, leanPercentage;

    //do a barrel roll
    private float rollDegrees = 0.0f;
    private int rollDir = 0;
    [SerializeField] float angleOfRoll = 15f;

    //inputs
    [HideInInspector] public float horz, vert, accel, drift, stunt;

    //camera controls
    [HideInInspector] public float inputAngle;
    private float cameraAngle;
    private Vector3 prevPos, prevUp;

    //HUD
    ShipHUD HUD;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        HUD = GetComponent<ShipHUD>();

        ship = Instantiate(shipPrefab, transform).transform;
        ship.localPosition = Vector3.zero;
        rb.mass = handling.mass;
        rb.drag = handling.drag;
        rb.angularDrag = handling.angularDrag;

        //Quick Fix: Change this to something proper
        if (gameObject.CompareTag("Player"))
        {
            cam = Instantiate(camPrefab, transform.position, transform.rotation).transform.GetComponent<Camera>();

            camSmooth = Instantiate(camSpot, transform.position, transform.rotation);
            camSnappy = Instantiate(camSpot, transform);
        }

        prevPos = ship.position;
        prevUp = ship.up;

        foreach (Transform child in ship.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name == "ShipModel")
            {
                model = child;
            }
        }

        g_manager = FindObjectOfType<GameManager>();
        gravityScalar = g_manager.GetGravity();
    }

    void FixedUpdate()
    {
        HoverLogic();
        ShipRoll(horz);
        RotationLogic(horz);
        Acceleration(accel);

        //Quick Fix: Change this to something proper
        if (gameObject.CompareTag("Player"))
            CameraFollow();
    }

    void HoverLogic()
    {
        Debug.DrawLine(transform.position, transform.position + newGravity.normalized * handling.desiredHeight, Color.blue);
        //get the current surface i am casting it from slightly higher to stop the ship 'unsticking' when it hits the floor
        RaycastHit hit;
        float castUp = 1.0f;
        Vector3 oldGrav = newGravity;
        if (Physics.Raycast(transform.position - newGravity.normalized * castUp, newGravity, out hit, handling.castDistance + castUp,groundLayer))
        {
            //adjust gravity to new surface
            newGravity = -hit.normal.normalized;
            newGravity *= gravityScalar;

            float hitDist = hit.distance - castUp;
            float currentUp = Vector3.Dot(rb.velocity, ship.up);
            float force = handling.desiredHeight - hitDist;

            if (hitDist <= handling.desiredHeight)
            {
                force *= (handling.maxHoverForce / handling.desiredHeight);
                if (force < 0) force *= -1.0f;
                force += 1.0f;

                if (currentUp <= 0.0f)
                {
                    force *= 2.0f;
                }
                else
                {
                    force *= 0.5f;
                }
            }
            else
            {
                if (force > 0) force *= -1.0f;

                if (currentUp <= 0.0f)
                {
                    force = 0.0f;
                }
            }

            rb.AddForce(force * -newGravity * rb.mass);
        }
        else
        {
            //reset to defaults
            newGravity = new Vector3(0.0f, -gravityScalar, 0.0f);
        }

        angleChange = Mathf.Lerp(angleChange, Vector3.SignedAngle(oldGrav, newGravity, ship.right), 0.1f);
    }

    void ShipRoll(float horzInput)
    {
        if (stunt > 0.0f && rollDegrees <= 0.0f)
        {
            rollDegrees = 360.0f;
            if (horzInput < 0.0f) rollDir = 1;
            else rollDir = -1;
        }

        model.RotateAroundLocal(Vector3.forward, -(Mathf.Deg2Rad * rollDegrees * rollDir));
        rollDegrees -= Time.fixedDeltaTime * 200.0f + Mathf.Abs(Mathf.Sin(0.5f * rollDegrees * Mathf.Deg2Rad)) * 1000.0f * Time.fixedDeltaTime;
        if (rollDegrees < 0.0f)
        {
            rollDegrees = 0.0f;
        }
        model.RotateAroundLocal(Vector3.forward, (Mathf.Deg2Rad * rollDegrees * rollDir));
    }

    void RotationLogic(float horzInput)
    {
        rotatePercentage = Mathf.Lerp(rotatePercentage, vert, Time.fixedDeltaTime * 6f);//3.5

        if (leanPercentage < horzInput)
        {
            if (horzInput <= 0.0f)
            {
                leanPercentage = Mathf.Lerp(leanPercentage, -horzInput, Time.fixedDeltaTime * 1.5f);
            }
            else
            {
                leanPercentage = Mathf.Lerp(leanPercentage, -horzInput, Time.fixedDeltaTime * 2.5f);
            }
        }
        else if (leanPercentage > horzInput)
        {
            if (horzInput >= 0.0f)
            {
                leanPercentage = Mathf.Lerp(leanPercentage, -horzInput, Time.fixedDeltaTime * 1.5f);
            }
            else
            {
                leanPercentage = Mathf.Lerp(leanPercentage,-horzInput, Time.fixedDeltaTime * 2.5f);
            }
        }

        ship.RotateAround(ship.forward, -prevLean);
        ship.RotateAround(ship.right, -prevRotate);
        ship.RotateAround(ship.right, -vibrate);

        //keep the forward momentum after turning
        currentSpeed = Vector3.Dot(rb.velocity, ship.forward);
        Vector3 oldPos = ship.position;
        Quaternion oldRot = ship.rotation;

        ship.position = rb.velocity;
        ship.RotateAround(Vector3.zero, ship.up, horzInput * handling.steerSpeed * (Time.fixedDeltaTime / 0.02f));
        rb.velocity = Vector3.Lerp(rb.velocity, ship.position, handling.steerMomentum);
        ship.position = oldPos;
        ship.rotation = oldRot;

        ship.RotateAround(ship.up, horzInput * handling.steerSpeed * Time.fixedDeltaTime);

        Vector3 vel = rb.velocity;
        Vector3 speedUp = Vector3.Project(vel, ship.up);
        Vector3 speedRight = Vector3.Project(vel, ship.right);
        Vector3 speedFwd = Vector3.Project(vel, ship.forward);

        speedFwd = speedFwd.normalized * Mathf.Abs(currentSpeed);
        vel = speedUp + speedRight + speedFwd;
        rb.velocity = Vector3.Lerp(rb.velocity, vel, handling.forwardMomentum);

        Vector3 proj = ship.forward.normalized - (Vector3.Dot(ship.forward, -newGravity.normalized)) * -newGravity.normalized;
        Quaternion newRot = Quaternion.LookRotation(proj.normalized, -newGravity.normalized);
        Quaternion finalRot = Quaternion.Lerp(ship.rotation, newRot, 6.0f * Time.fixedDeltaTime);
        ship.rotation = finalRot;

        AirBrake();

        prevRotate = (Mathf.Deg2Rad * rotatePercentage) * handling.pitchLimit;
        prevLean = (Mathf.Deg2Rad * leanPercentage) * (30.0f * (currentSpeed / handling.speed) + 20.0f);
        vibrate = (handling.speed - Mathf.Abs(currentSpeed)) / handling.speed;
        if (vibrate < 0.0f) vibrate = 0.0f;
        vibrate *= Mathf.Sin(Time.time * 100.0f);
        vibrate *= Mathf.Deg2Rad * 0.35f;

        ship.RotateAround(ship.right, vibrate);
        ship.RotateAround(ship.right, prevRotate);
        ship.RotateAround(ship.forward, prevLean);

    }

    void AirBrake()
    {
        //braking to prevent drifts
        float accelForce = -Vector3.Dot(rb.velocity, ship.right) * handling.airBrake;
        accelForce *= (1.0f - drift);
        rb.AddForce(ship.right * accelForce * rb.mass);
    }

    void Acceleration(float accelInput)
    {
        if(Vector3.Dot(rb.velocity, newGravity) > 0.0f) ForceWithoutDrag(newGravity);
        else rb.AddForce(newGravity * rb.mass);

        currentSpeed = Vector3.Dot(rb.velocity, ship.forward);
        HUD.UpdateSpeed(currentSpeed);

        if (accelInput == 0.0f && drift > 0.0f && handling.driftForward) return;
        if ((accelInput > 0.0f && currentSpeed > handling.speed) || (accelInput < 0.0f && currentSpeed < -handling.reverseSpeed)) return; //for booster pads

        float ratio = 0.0f;
        float accelForce = -currentSpeed;

        //Apply deceleration when forward input is not pressed
        if (accelInput == 0.0f)
        {
            if (drift > 0.0f && handling.driftForward)
            {
                accelForce = 0.0f;
            }
            accelForce *= handling.deceleration;
        }

        if (accelInput > 0.0f) //Forward acceleration
        {
            ratio = currentSpeed / handling.speed;
            ratio = 1.0f - ratio;
            accelForce = handling.accelerationMin;
            accelForce += (handling.acceleration - handling.accelerationMin) * ratio * accelInput;
        }
        else if (accelInput < 0.0f) //Reversing
        {
            ratio = currentSpeed / -handling.reverseSpeed;
            ratio = 1.0f - ratio;
            accelForce = handling.brake * ratio * accelInput;
        }

        //applying force to the ship to move
        ForceWithoutDrag(accelForce * ship.forward);
    }

    void ForceWithoutDrag(Vector3 force)
    {
        //https://forum.unity.com/threads/physics-drag-formula.252406/
        float coeff = (1.0f - Time.fixedDeltaTime * rb.drag);
        rb.AddForce(((force + Vector3.Dot(rb.velocity, force.normalized) * force.normalized * rb.drag) / coeff) * rb.mass);
    }

    void CameraFollow()
    {
        cam.transform.RotateAround(prevPos, prevUp, -cameraAngle);
        float dist = inputAngle - cameraAngle;
        if (dist >= 180.0f) cameraAngle += 360.0f;
        if (dist <= -180.0f) cameraAngle -= 360.0f;
        cameraAngle = Mathf.Lerp(cameraAngle, inputAngle, Time.fixedDeltaTime * 7.5f);

        float vel = rb.velocity.magnitude;

        if(currentSpeed < 0.0f && accel <= 0.0f)
        {
            vel /= handling.reverseSpeed;
        }
        else
        {
            vel /= handling.speed;
        }

        Vector3 diff = cam.transform.position - ship.transform.position;
        float x = diff.magnitude;
        float y = Vector3.Dot(diff, ship.up);
        int mult = 1;
        if (y < 0.0f) mult = -1;

        Vector3 newPos = transform.position - (camBackInit + vel * camBackExtra) * ship.forward + camUp * ship.up + camRight * ship.right;
        Vector3 camVel = Vector3.zero;
        camSmooth.position = Vector3.SmoothDamp(camSmooth.position, newPos, ref camVel, 0.06f);

        camVel = Vector3.zero;
        camSnappy.position = Vector3.SmoothDamp(camSnappy.position, newPos, ref camVel, 0.06f);
        Vector3 oldPos = cam.transform.position;

        cam.transform.position = Vector3.Lerp(camSnappy.position, camSmooth.position, camSmoothing);

        Quaternion oldRot = cam.transform.rotation;
        Quaternion newRot = Quaternion.LookRotation(ship.forward, ship.up);

        float angle = Mathf.Asin(y / x) * Mathf.Rad2Deg;
        angle *= camRotate * mult;

        newRot = Quaternion.Lerp(newRot, Quaternion.LookRotation(-ship.up * mult, ship.forward * mult), angle / 90.0f);
        cam.transform.rotation = Quaternion.Lerp(oldRot, newRot, camTurnRotation * (1.0f + vel) * Time.fixedDeltaTime); //camTurnRotation * (1.0f + vel) * Time.fixedDeltaTime

        //cam.transform.RotateAround(ship.position, prevUp, cameraAngle);
        prevPos = ship.position;
        prevUp = Vector3.Lerp(prevUp, ship.up, 0.15f);
    }

    public int GetID()
    {
        return shipID;
    }

    public float GetMaxSpeed()
    {
        return handling.speed;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public float GetSidewaysSpeed()
    {
        return Vector3.Dot(rb.velocity, ship.right);
    }

    public float GetFallSpeed()
    {
        return Vector3.Dot(rb.velocity, ship.up);
    }

    public Transform GetShipTransform()
    {
        return ship.transform;
    }

    public float GetAcceleration()
    {
        return accel;
    }

    public float GetSteer()
    {
        return horz * handling.steerSpeed;
    }

    public float GetPitch()
    {
        return vert * handling.pitchLimit;
    }

    public float AngleChange()
    {
        return angleChange;
    }

    public void Respawn()
    {
        transform.position = previousCheckpoint.position + new Vector3(0,-5,0);
    }
}
