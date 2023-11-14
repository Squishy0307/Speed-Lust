using System;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour {

    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;

    [SerializeField] public List<GameObject> carTransformList;

    private List<CheckpointSingle> checkpointSingleList;
    public List<int> nextCheckpointSingleIndexList;

    private void Awake() {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        checkpointSingleList = new List<CheckpointSingle>();
        foreach (Transform checkpointSingleTransform in checkpointsTransform) {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();

            checkpointSingle.SetTrackCheckpoints(this);

            checkpointSingleList.Add(checkpointSingle);
        }

        nextCheckpointSingleIndexList = new List<int>();
        foreach (GameObject carTransform in carTransformList) {
            nextCheckpointSingleIndexList.Add(0);
        }
    }

    public void Start()
    {
        setShipPosition();
    }

    public void Update()
    {

    }

    public void setShipPosition()
    {
        for(int i = 0; i < carTransformList.Count; i++)
        {
            carTransformList[i].GetComponent<VehicleMovement>().CarPosition = i + 1;
            carTransformList[i].GetComponent<VehicleMovement>().carNumber = i;
        }
    }

    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle, GameObject carTransform, int carNumber) {
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)];
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex) {
            // Correct checkpoint
            Debug.Log("Correct");
            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctCheckpointSingle.Hide();

            nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)]
                = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
        } else {
            // Wrong checkpoint
            Debug.Log("Wrong");
            OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);

            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            //correctCheckpointSingle.Show();

            
        }
        comparePositions(carNumber);
    }


    public void comparePositions(int carNumber)
    {
        if (carTransformList[carNumber].GetComponent<VehicleMovement>().CarPosition > 1)
        {
            GameObject currentCar = carTransformList[carNumber];
            int currentCarPos = currentCar.GetComponent<VehicleMovement>().CarPosition;
            int currentCarCP = currentCar.GetComponent<VehicleMovement>().checkpointCount;

           // Debug.Log("currentCarCP = " + currentCarCP);

            GameObject carInFront = null;
            int carInFrontPos = 0;
            int carInFrontCP = 0;

            for(int i = 0; i < carTransformList.Count; i++)
            {
                if (carTransformList[i].GetComponent<VehicleMovement>().CarPosition == currentCarPos - 1)
                {
                    carInFront = carTransformList[i];
                    carInFrontCP = carInFront.GetComponent<VehicleMovement>().checkpointCount;
                    carInFrontPos = carInFront.GetComponent<VehicleMovement>().CarPosition;
                    //Debug.Log("carInFrontCP = " + carInFrontCP);
                    break;
                }
            }

            if(currentCarCP > carInFrontCP)
            {
                currentCar.GetComponent<VehicleMovement>().CarPosition = currentCarPos - 1;
                carInFront.GetComponent<VehicleMovement>().CarPosition = carInFrontPos + 1;

                Debug.Log("Car " + carNumber + "has more checkpoints than " + "Car " + carInFront.GetComponent<VehicleMovement>().carNumber);

            }
        }
    }

}
