using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    CheckpointSingle checkpointSingle;
    ShipController shipController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ShipController>(out ShipController ship))
        {

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 6)
        {
            shipController = other.gameObject.GetComponent<ShipController>();

            shipController.Respawn();
        }
    }

}
