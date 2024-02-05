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
    public float lerpSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        vehicleMovement = player.GetComponent<VehicleMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
        speedSlider.value = Mathf.Lerp(speedSlider.value, playerSpeed.currentSpd + 50, lerpSpeed * Time.deltaTime);
        player.GetComponent<AudioSource>().pitch = 1 + ((speedSlider.value - 50) / 1000);

        //if (speedSlider.value <= 59)
        //{
        //    speedSlider.value = 59;
        //}
    }
}
