using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ShipSelectDisplay : MonoBehaviour
{
    public GameObject ship1;
    public GameObject ship2;
    public GameObject ship3;
    public UnityEvent onSelect;

    public Material[] ShipLogo;
    public MeshRenderer DisplayRoomWall;

    // Start is called before the first frame update
    void Start()
    {
        ship2.SetActive(false);
        ship3.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HighlightShip1()
    {
        ship1.SetActive(true);
        ship2.SetActive(false);
        ship3.SetActive(false);
        ShipSelector.Instance.currentShipIndex = 0;
        DisplayRoomWall.material = ShipLogo[0];
    }

    public void HighlightShip2()
    {
        ship1.SetActive(false);
        ship2.SetActive(true);
        ship3.SetActive(false);
        ShipSelector.Instance.currentShipIndex = 1;
        DisplayRoomWall.material = ShipLogo[1];
    }

    public void HighlightShip3()
    {
        ship1.SetActive(false);
        ship2.SetActive(false);
        ship3.SetActive(true);
        ShipSelector.Instance.currentShipIndex = 2;
        DisplayRoomWall.material = ShipLogo[2];
    }
}
