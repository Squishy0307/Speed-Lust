using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipSelector : MonoBehaviour
{
    public static ShipSelector Instance;

    public TextMeshProUGUI shipName;

    public GameObject currentShip;
    public GameObject[] ships;

    private int currentShipIndex = 0;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        currentShip = ships[currentShipIndex];
        shipName.text = currentShip.name;

        for (int i = 0; i < ships.Length; i++) 
        {
            if (i == 0)
            {
                ships[i].SetActive(true);
                return;
            }

            ships[i].SetActive(false);
        }
    }
    
    public void NextShip()
    {
        ships[currentShipIndex].SetActive(false);
        currentShipIndex++;

        if(currentShipIndex >= ships.Length)
        {
            currentShipIndex = 0;
        }

        ships[currentShipIndex].SetActive(true);

        currentShip = ships[currentShipIndex];
        shipName.text = currentShip.name;
    }

    public void PreviousShip()
    {
        ships[currentShipIndex].SetActive(false);
        currentShipIndex--;

        if (currentShipIndex < 0)
        {
            currentShipIndex = ships.Length - 1;
        }

        ships[currentShipIndex].SetActive(true);

        currentShip = ships[currentShipIndex];
        shipName.text = currentShip.name;
    }


}
