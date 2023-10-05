using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private ShipController ship;

    private float horz, vert, accel, drift, stunt;
    private float inputAngle;

    void Start()
    {
        ship = GetComponent<ShipController>();
    }

    
    void Update()
    {
        horz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
        accel = Input.GetAxis("Acceleration");
        drift = Input.GetAxis("Drift");
        stunt = Input.GetAxis("Stunt");

        inputAngle = Mathf.Atan2(Input.GetAxis("RightX"), Input.GetAxis("RightY")) * Mathf.Rad2Deg;

        ship.horz = horz;
        ship.vert = vert;
        ship.accel = accel;
        ship.drift = drift;
        ship.stunt = stunt;
        ship.inputAngle = inputAngle;

    }
}
