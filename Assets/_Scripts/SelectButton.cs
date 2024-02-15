using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{

    public UnityEvent onPress;
    public Animator button;
    public GameObject thisShip;
    public GameObject otherShip1;
    public GameObject otherShip2;
    public Material ShipLogo;
    public MeshRenderer DisplayRoomWall;

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            //Debug.Log("selected");
            thisShip.SetActive(true);
            otherShip1.SetActive(false);
            otherShip2.SetActive(false);
            DisplayRoomWall.material = ShipLogo;
        }
        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            Debug.Log("Pressed");
            onPress.Invoke();
        }
    }
}
