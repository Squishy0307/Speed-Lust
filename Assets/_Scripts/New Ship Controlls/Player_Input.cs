using UnityEngine;

public class Player_Input : MonoBehaviour
{
    public string verticalAxisName = "Vertical";        //The name of the thruster axis
    public string horizontalAxisName = "Horizontal";    //The name of the rudder axis
    public string brakingKey = "Brake";                 //The name of the brake button

    private float thruster;                             //The current thruster value
    private float rudder;                               //The current rudder value
    private bool isBraking;                             //The current brake value

    private VehicleMovement vehicalMovement;

    private void Awake()
    {
        vehicalMovement = GetComponent<VehicleMovement>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !Application.isEditor)
            Application.Quit();

        thruster = Input.GetAxis(verticalAxisName);
        rudder = Input.GetAxis(horizontalAxisName);
        isBraking = Input.GetButton(brakingKey);
        vehicalMovement.SetInputs(rudder,thruster,isBraking);
    }
}