using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float startTime;
    private float elapsedTime;
    private bool hasStartedLap = false;

    public GameObject lapText;
    public GameObject prevLapText;
    public GameObject bestLap;
    public List<float> bestTimes = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStartedLap)
        {

            elapsedTime = Time.time - startTime;
            //Debug.Log(elapsedTime);

            lapText.GetComponent<Text>().text = "Lap Time: " + elapsedTime.ToString("00:00:00");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasStartedLap = true;
            startTime = Time.time;

            prevLapText.GetComponent<Text>().text = "Last Lap: " + elapsedTime.ToString("00:00:00");

            if (other.name == "chk (60)")
            {

                bestTimes.Add(elapsedTime);

            }

            if (bestTimes.Count > 0)
            {

                bestTimes.Sort();
                bestLap.GetComponent<Text>().text = "Best Lap: " + bestTimes[0].ToString();

            }
        }
    }
}
