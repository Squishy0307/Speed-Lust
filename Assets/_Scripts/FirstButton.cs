using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FirstButton : MonoBehaviour
{

    Button button;
    public ShipSelectDisplay shipSelectDisplay;
    private PlayerControlls playerControlls;

    public MeshRenderer DisplayRoomWall;

    public bool buttonPressed;


    private bool canMove = true;
    public float currentShip = 0;

    [SerializeField] Animator[] shipBtnUI;
    private bool isLoadingScene = false;

    private void Awake()
    {
        //button = GetComponent<Button>();
        //button.Select();
        playerControlls = new PlayerControlls();
        shipBtnUI[0].SetTrigger("Selected");
    }

    private void Update()
    {
        float x = playerControlls.ShipControls.UIMove.ReadValue<Vector2>().x;

        if (x > 0 && canMove)
        {
            canMove = false;
            Debug.Log("moved Right");

            currentShip++;
            if (currentShip > 2)
            {
                currentShip = 0;
            }

            for (int i = 0; i < shipBtnUI.Length; i++)
            {
                if (i == currentShip)
                {
                    shipBtnUI[i].SetTrigger("Selected");
                }
                else
                {
                    shipBtnUI[i].SetTrigger("Normal");
                }
            }

        }
        else if (x < 0 && canMove)
        {
            canMove = false;
            Debug.Log("moved left");

            currentShip--;
            if (currentShip < 0)
            {
                currentShip = 2;
            }

            for (int i = 0; i < shipBtnUI.Length; i++)
            {
                if (i == currentShip)
                {
                    shipBtnUI[i].SetTrigger("Selected");
                }
                else
                {
                    shipBtnUI[i].SetTrigger("Normal");
                }
            }
        }
        else if (x == 0)
        {
            canMove = true;
        }

        if (currentShip == 0)
        {
            shipSelectDisplay.HighlightShip1();
        }
        else if (currentShip == 1)
        {
            shipSelectDisplay.HighlightShip2();
        }
        else if (currentShip == 2)
        {
            shipSelectDisplay.HighlightShip3();
        }


        if (playerControlls.ShipControls.Select.IsPressed() && !isLoadingScene)
        {
            isLoadingScene = true;
            Scene_Manager.Instance.LoadScene(2);
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