using UnityEngine;
using Cinemachine;

public class Player_Input : MonoBehaviour
{
    private PlayerControlls playerControlls;

    //public string verticalAxisName = "Vertical";        //The name of the thruster axis
    //public string horizontalAxisName = "Horizontal";    //The name of the rudder axis
    //public string brakingKey = "Brake";                 //The name of the brake button

    private float thruster;                             //The current thruster value
    private float rudder;                               //The current rudder value
    private bool isBraking;                             //The current brake value

    private VehicleMovement vehicalMovement;
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] float camSmooth = 1f;
    [SerializeField] AnimationCurve camSmoothEase;
    CinemachineTransposer transposer;
    CinemachineComposer composer;

    [SerializeField] GameObject Buttons;

    private void Awake()
    {
        vehicalMovement = GetComponent<VehicleMovement>();
        transposer = cam.GetCinemachineComponent<CinemachineTransposer>();
        composer = cam.GetCinemachineComponent<CinemachineComposer>();

        playerControlls = new PlayerControlls();
        Buttons.SetActive(false);
    }

    private void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !Application.isEditor)
            Application.Quit();

        thruster = playerControlls.ShipControls.Thruster.ReadValue<float>();
        rudder = playerControlls.ShipControls.Rudder.ReadValue<Vector2>().x;
        isBraking = playerControlls.ShipControls.Brake.IsPressed();

        //thruster = Input.GetAxis(verticalAxisName);
        //rudder = Input.GetAxis(horizontalAxisName);
        //isBraking = Input.GetButton(brakingKey);


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

        if (Buttons.activeSelf)
        {
            if (playerControlls.ShipControls.Select.IsPressed())
            {
                Debug.Log("PressedA");
                Scene_Manager.Instance.LoadScene(2);
            }
            if (playerControlls.ShipControls.Exit.IsPressed())
            {
                Debug.Log("PressedB");
                Scene_Manager.Instance.LoadScene(1);
            }
        }

    }

    private void OnEnable()
    {
        playerControlls.Enable();
    }

    private void OnDisable()
    {
        playerControlls.Disable();
    }

}