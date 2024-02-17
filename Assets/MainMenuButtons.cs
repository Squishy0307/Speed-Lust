using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    public bool buttonPressed;
    public bool exitPressed;
    private PlayerControlls playerControlls;
    public UnityEvent onPress;


    private void Awake()
    {
        playerControlls = new PlayerControlls();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        buttonPressed = playerControlls.ShipControls.Select.IsPressed();
        exitPressed = playerControlls.ShipControls.Exit.IsPressed();

        if (buttonPressed)
        {
            Debug.Log("Pressed");
            onPress.Invoke();
        }

        if (exitPressed)
        {
            Application.Quit();
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
