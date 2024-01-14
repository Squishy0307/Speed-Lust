using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Vibration_Manager : MonoBehaviour
{
    public static Vibration_Manager Instance;

    private Gamepad gp;
    private bool vibrating = false;

    public bool gamepadConnected = false;
    private bool canRumble = true;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        else
        {
            Destroy(gameObject);
        }

        if (InputSystem.GetDevice<Gamepad>() == null)
        {
            gamepadConnected = false;
        }

        else
        {
            gamepadConnected = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gp = InputSystem.GetDevice<Gamepad>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gp == null)
        {
            gp = InputSystem.GetDevice<Gamepad>();
        }

        if(InputSystem.GetDevice<Gamepad>() == null)
        {
            gamepadConnected = false;
        }

        else
        {
            gamepadConnected = true;
        }
    }

    public void VibrateNow(float leftMfrq, float rightMfrq, float duration)
    {
        if(canRumble)
            StartCoroutine(rumble(leftMfrq,rightMfrq,duration));
    }


    IEnumerator rumble(float leftMfrq, float rightMfrq, float duration)
    {
        if (!vibrating && gp != null)
        {
            vibrating = true;       
            InputSystem.ResumeHaptics();
            gp.SetMotorSpeeds(leftMfrq, rightMfrq);

            yield return new WaitForSecondsRealtime(duration);

            InputSystem.ResetHaptics();
            vibrating = false;
        }
    }

    public void changeRumbleSetting(bool CanRumble)
    {
        canRumble = CanRumble;
    }

}

