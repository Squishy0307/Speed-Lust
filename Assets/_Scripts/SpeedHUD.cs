using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SpeedHUD : MonoBehaviour
{
    private VehicleMovement vehicleMovement;
    public GameObject player;
    public Slider speedSlider;
    public ShipHUD playerSpeed;

    // Start is called before the first frame update
    void Start()
    {
        vehicleMovement = player.GetComponent<VehicleMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        speedSlider.value = playerSpeed.currentSpd;
    }
}
