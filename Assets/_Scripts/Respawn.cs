using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    public float threshold = 5;

    private TrackCheckpoints trackCheckpoints;

    public Vector3 spawnPoint;

    //public Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        //spawnPoint
    }

    private void Update()
    {
        threshold -= Time.deltaTime;

        if(threshold <= 0 )
        {
            //transform.position = trackCheckpoints.lastCheckpoint;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //other.transform.position = spawnPoint;
    }



}
