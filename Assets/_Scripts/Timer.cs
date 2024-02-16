using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public float startTime;
    public float elapsedTime;
    public bool hasStartedLap = false;
    public bool hasEnteredCheckpoint = false;

    public TextMeshProUGUI lapText;
    public TextMeshProUGUI prevLapText;
    public TextMeshProUGUI bestLap;
    public float overallBestTime;
    public List<float> bestTimes = new List<float>();

    public CheckpointTracker tracker;

    private StartCountdown startCountdown;

    private CheckpointTracker checkpointTracker;
    public float currentAIBestTime;

    public LeaderboardTimes LBT;

    // Start is called before the first frame update
    void Start()
    {
        startCountdown = FindObjectOfType<StartCountdown>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStartedLap && !startCountdown.timerStarted && GameManager.Instance.RaceStarted)
        {

            elapsedTime = Time.time - startTime;
            //Debug.Log(elapsedTime);


            lapText.text = FormatSeconds(elapsedTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasEnteredCheckpoint && !startCountdown.timerStarted)
        {
            hasEnteredCheckpoint = true;
            //prevLapText.text = "Last Lap: " + elapsedTime.ToString("00:00");
            hasStartedLap = true;
            startTime = Time.time;

            

            if (elapsedTime >= 1 && tracker.finishLinePass>=1)
            {
                //Debug.Log("Working");
                bestTimes.Add(elapsedTime);

            }

            if (bestTimes.Count > 0)
            {

                bestTimes.Sort();
                overallBestTime = bestTimes[0];
                int minutes = Mathf.FloorToInt(overallBestTime / 60);
                int seconds = Mathf.FloorToInt(overallBestTime % 60);
                bestLap.text = string.Format("{0:00}:{1:00}", minutes, seconds);

                //bestLap.text = bestTimes[0].ToString("{00}");

            }
        }

        if (other.CompareTag("AI") && !hasEnteredCheckpoint )
        {
            hasEnteredCheckpoint = true;
            //other.gameObject.GetComponent<CheckpointTracker>().prevLapTextAI.text = "Last Lap: " + other.gameObject.GetComponent<CheckpointTracker>().elapsedTime.ToString("00:00");
            hasStartedLap = true;
            startTime = Time.time;

            //LBT.DoLeaderboard();

            if (other.gameObject.GetComponent<CheckpointTracker>().elapsedTime >= 1 && other.gameObject.GetComponent<CheckpointTracker>().finishLinePass >= 1)
            {
                //Debug.Log("Working");
                bestTimes.Add(other.gameObject.GetComponent<CheckpointTracker>().elapsedTime);

            }

            if (bestTimes.Count > 0)
            {

                bestTimes.Sort();
                //other.gameObject.GetComponent<CheckpointTracker>().bestLapAI.text = "Best Lap: " + bestTimes[0].ToString("00:00");
                currentAIBestTime = bestTimes[0];

            }
        }

        else if(!hasEnteredCheckpoint && !startCountdown.timerStarted)
        {
            checkpointTracker = other.GetComponent<CheckpointTracker>();
            checkpointTracker.startTime = Time.time;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        hasEnteredCheckpoint = false;
    }

    string FormatSeconds(float elapsed)
    {
        int d = (int)(elapsed * 100.0f);
        int minutes = d / (60 * 100);
        int seconds = (d % (60 * 100)) / 100;
        int hundredths = d % 100;
        return String.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, hundredths);
    }
}
