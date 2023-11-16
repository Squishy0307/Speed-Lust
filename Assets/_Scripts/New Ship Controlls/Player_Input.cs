using UnityEngine;
using Cinemachine;

public class Player_Input : MonoBehaviour
{
    public string verticalAxisName = "Vertical";        //The name of the thruster axis
    public string horizontalAxisName = "Horizontal";    //The name of the rudder axis
    public string brakingKey = "Brake";                 //The name of the brake button

    private float thruster;                             //The current thruster value
    private float rudder;                               //The current rudder value
    private bool isBraking;                             //The current brake value

    private VehicleMovement vehicalMovement;
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] float camSmooth = 1f;
    [SerializeField] AnimationCurve camSmoothEase;
    CinemachineTransposer transposer;
    CinemachineComposer composer;

    private void Awake()
    {
        vehicalMovement = GetComponent<VehicleMovement>();
        transposer = cam.GetCinemachineComponent<CinemachineTransposer>();
        composer = cam.GetCinemachineComponent<CinemachineComposer>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !Application.isEditor)
            Application.Quit();

        thruster = Input.GetAxis(verticalAxisName);
        rudder = Input.GetAxis(horizontalAxisName);
        isBraking = Input.GetButton(brakingKey);
        vehicalMovement.SetInputs(rudder,thruster,isBraking);

        if (vehicalMovement.GetCurrentSpeed() >= 60)
        {
            transposer.m_FollowOffset.x = Mathf.Lerp(transposer.m_FollowOffset.x, rudder, Time.deltaTime * camSmooth);
            composer.m_TrackedObjectOffset.x = Mathf.Lerp(composer.m_TrackedObjectOffset.x, rudder, camSmoothEase.Evaluate(Time.deltaTime * camSmooth));
        }
        else
        {
            transposer.m_FollowOffset.x = Mathf.Lerp(transposer.m_FollowOffset.x, 0, Time.deltaTime * camSmooth);
            composer.m_TrackedObjectOffset.x = Mathf.Lerp(composer.m_TrackedObjectOffset.x, 0, camSmoothEase.Evaluate(Time.deltaTime * camSmooth));
        }
    }
}