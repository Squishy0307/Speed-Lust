using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float startTime;
    public float elapsedTime;
    public bool hasStartedLap = false;
    public bool hasEnteredCheckpoint = false;

    public GameObject lapText;
    public GameObject prevLapText;
    public GameObject bestLap;
    public List<float> bestTimes = new List<float>();

    public CheckpointTracker tracker;

    private StartCountdown startCountdown;

    // Start is called before the first frame update
    void Start()
    {
        startCountdown = FindObjectOfType<StartCountdown>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStartedLap && !startCountdown.timerStarted)
        {

            elapsedTime = Time.time - startTime;
            //Debug.Log(elapsedTime);

            lapText.GetComponent<Text>().text = "Lap Time: " + elapsedTime.ToString("00:00:00");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasEnteredCheckpoint && !startCountdown.timerStarted)
        {
            hasEnteredCheckpoint = true;
            prevLapText.GetComponent<Text>().text = "Last Lap: " + elapsedTime.ToString("00:00:00");
            hasStartedLap = true;
            startTime = Time.time;

            

            if (elapsedTime >= 1 && tracker.finishLinePass>=1)
            {
                Debug.Log("Working");
                bestTimes.Add(elapsedTime);

            }

            if (bestTimes.Count > 0)
            {

                bestTimes.Sort();
                bestLap.GetComponent<Text>().text = "Best Lap: " + bestTimes[0].ToString("00:00:00");

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        hasEnteredCheckpoint = false;
    }
}
