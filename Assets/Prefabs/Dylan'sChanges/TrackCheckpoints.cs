using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrackCheckpoints : MonoBehaviour
{
    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;

    [SerializeField] private List<Transform> shipTransformList;
    public List<CheckpointSingle> checkpointSingleList;
    //public Vector3 lastCheckpoint;

    //private Respawn respawn;

    private List<int> nextCheckpointSingleIndexList;
    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        checkpointSingleList = new List<CheckpointSingle>();

        foreach(Transform checkpointSingleTransform in checkpointsTransform)
        {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();

            checkpointSingle.SetTrackCheckpoints(this);

            checkpointSingleList.Add(checkpointSingle);
        }

        nextCheckpointSingleIndexList = new List<int>();
        foreach(Transform shipTransform in shipTransformList)
        {
            nextCheckpointSingleIndexList.Add(0);
        }
    }

    public void Update()
    {
        
    }

    public void ShipThroughCheckpoint(CheckpointSingle checkpointSingle, Transform shipTransform)
    {

        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[shipTransformList.IndexOf(shipTransform)];
        if(checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            Debug.Log("Correct");

            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctCheckpointSingle.Hide();

            nextCheckpointSingleIndexList[shipTransformList.IndexOf(shipTransform)] = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            
            OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Debug.Log("Wrong");
            OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);
            
            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctCheckpointSingle.Show();
        }

        //lastCheckpoint = checkpointSingle.transform.position;
    }
}
