using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;
    private MeshRenderer meshRenderer;
    public Transform nextCheckpoint;
    private Transform previousCheckpoint;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        Hide();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ShipController>(out ShipController ship))
        {
            trackCheckpoints.CarThroughCheckpoint(this, other.transform);
            Debug.Log(other.name);

            previousCheckpoint = this.transform;

            ship.SetCheckPoints(nextCheckpoint, previousCheckpoint);
            ship.checkpointCount++;

            //TO DO: Change the 3 to check the count of number of points in the checkpointList;

            if (ship.checkpointCount >=3) 
            {
                //TO DO: Check if the current lap is final lap (Used to enable Mega boost)

                ship.checkpointCount = 0; 
                ship.lapNumber = "Lap 2";
            }
        }

    }

    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints)
    {
        this.trackCheckpoints = trackCheckpoints;
    }

    public void Show()
    {
        meshRenderer.enabled = true;
    }

    public void Hide()
    {
        meshRenderer.enabled = false;
    }
}
