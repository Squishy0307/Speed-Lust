using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectButton2 : MonoBehaviour
{
    private PlayerControlls playerControlls;

    public UnityEvent onPress;
    private Animator button;
    //public GameObject thisShip;
    //public GameObject otherShip1;
    //public GameObject otherShip2;
    //public Material ShipLogo;
    //public MeshRenderer DisplayRoomWall; 

    //public bool buttonPressed;
    public bool exitPressed;

    private void Awake()
    {
        playerControlls = new PlayerControlls();
    }
    private void Start()
    {
        button = GetComponent<Animator>();
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            //Debug.Log("selected");
            //thisShip.SetActive(true);
            //otherShip1.SetActive(false);
            //otherShip2.SetActive(false);
            //DisplayRoomWall.material = ShipLogo;
        }

        //buttonPressed = playerControlls.ShipControls.Select.IsPressed();
        exitPressed = playerControlls.ShipControls.Exit.IsPressed();

        if (exitPressed)
        {
            Debug.Log("Pressed");
            onPress.Invoke();
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
