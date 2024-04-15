using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private CheckpointTracker tracker;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        //Fader.Instance.RespawnFade();
        Debug.Log("Working");
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("AI"))
        {
            tracker = other.GetComponent<CheckpointTracker>();
            tracker.respawnPoint = this.gameObject;
        }
    }
}
