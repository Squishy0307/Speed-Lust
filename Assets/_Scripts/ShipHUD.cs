using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipHUD : MonoBehaviour
{
    [SerializeField] private Canvas HUD;            //Canvas 
    [SerializeField] TextMeshProUGUI speedText;     //UI text element to show current speed

    private float speed;                            //Current Speed of the vehical
    private int currentLap;                         //Current Lap of the vehicle
    
    private int checkpointCount;                    //Total Checkpoints the vehicle has went through
    private Transform nextCheckpoint;               //Next checkpoint 
    private Transform previousCheckpoint;           //Previous checkpoint ------------[SET PRIVATE LATER]------------

    private VehicleMovement vehicle;                //Reference to the vehcile script

    public Text positionTracker;

	void Start()
    {
        //We only want to update the UI if the current vehicle is controlled by the player, removing the Canvas HUD will not call the update fn
        if (transform.gameObject.tag != "Player")
            HUD = null;
        
        if (!HUD) return;

        vehicle = GetComponent<VehicleMovement>();

        foreach (Transform child in HUD.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name == "Speed")
            {
                //speedText = child.GetComponent<TextMeshProUGUI>();
            }
        }
    }
	
	void Update()
    {
        //We only want to update the UI if the current vehicle is controlled by the player
        if (!HUD) return;

        speed = vehicle.GetCurrentSpeed();
        UpdateHUD();

        if(Input.GetKeyDown(KeyCode.F9)) //-----------[Remove Later]-----------
        {
            StartCoroutine(Test());
        }
	}

    void UpdateHUD()
    {
        // Convert the speed into KPH by multiplying the value by 3.6f
        float currentSpd = speed * 3.6f;//* 30f; //FAKE SPEED
        if (currentSpd > 0.5f)
        {
            speedText.text = currentSpd.ToString("F0") + " KPH";
        }
        else
        {
            speedText.text =  "0 KPH";
        }

        positionTracker.text = "Pos: " + gameObject.GetComponent<VehicleMovement>().CarPosition.ToString() + " / 2";
    }

    //Updates the checkpoints
    public void SetCheckPoints(Transform NextCheckpoint, Transform PreviousCheckpoint)
    {
        nextCheckpoint = NextCheckpoint;
        previousCheckpoint = PreviousCheckpoint;
    }

    public void Respawn()
    {
        if (previousCheckpoint != null)
        {
            transform.position = previousCheckpoint.position + new Vector3(0, -5, 0);
        }
        else
        {
            Debug.LogError("PREVIOUS CHECKPOINNT IS NULL!");
        }
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(0.5f);
        Respawn();
    }

    public void UpdateCheckpointCount()
    {
        checkpointCount++;
    }

    public int GetCheckpointCount()
    {
        return checkpointCount;
    }

    public void UpdateLap(int LapNumber)
    {
        currentLap = LapNumber;
    }
}
