using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiTrigger : MonoBehaviour
{
    public CheckpointTracker player;
    public GameObject[] confetti;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject fetti in confetti)
        {
            fetti.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Confetti();
        }
    }

    IEnumerator Confetti()
    {
        yield return new WaitForSeconds(3);
        if (player.finishLinePass >= 2)
        {
            foreach (GameObject fetti in confetti)
            {
                fetti.SetActive(true);
            }
        }
    }
}
