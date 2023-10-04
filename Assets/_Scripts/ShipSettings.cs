using UnityEngine;

[CreateAssetMenu(fileName = "New Ship Setting", menuName = "Ship Setting/ Ship")]
public class ShipSettings : ScriptableObject
{
    //controls how the ship handles
    [Header("Ship Handling")]
    public float desiredHeight = 1.8f;
    public float maxHoverForce = 50.0f;
    public float castDistance = 25.0f;
    public float speed = 100.0f;
    public float reverseSpeed = 75.0f;
    public float acceleration = 74.0f;
    public float accelerationMin = 7.0f;
    public float brake = 55.0f;
    public float deceleration = 0.5f;
    public float airBrake = 1.0f;
    public float steerSpeed = 31.5f;
    public float steerMomentum = 75.0f;
    public float forwardMomentum = 0.35f;
    public float pitchLimit = 6.5f; //degrees
    public bool driftForward = true;
    [Space(10)]
    public float mass = 1000.0f;
    public float drag = 0.25f;
    public float angularDrag = 5.0f;

}
