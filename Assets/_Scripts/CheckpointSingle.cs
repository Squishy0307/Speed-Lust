using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;
    private MeshRenderer meshRenderer;
    public Transform nextCheckpoint;
    public Transform previousCheckpoint;

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
            trackCheckpoints.ShipThroughCheckpoint(this, other.transform);
            Debug.Log(other.name);

            previousCheckpoint = this.transform;

            ship.nextCheckpoint = nextCheckpoint;
            ship.previousCheckpoint = previousCheckpoint;

            ship.checkpointCount++;

            if(ship.checkpointCount >=3)
            {
                ship.lapNumber = "Lap 2";
            }
        }

        //if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("AI"))
        //{
        //    trackCheckpoints.ShipThroughCheckpoint(this, other.transform);
        //}
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
