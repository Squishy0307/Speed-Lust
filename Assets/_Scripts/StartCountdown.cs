using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartCountdown : MonoBehaviour
{
    public TMP_Text countdownText;
    public GameObject countdown;
    public float currentTime = 0;
    public float startingTime = 3;
    public bool timerStarted = false;

    void Start()
    {
        countdown.SetActive(false);
        currentTime = startingTime;
        StartCoroutine(Countdown(3));
    }

    public void Update()
    {
        if(timerStarted)
        {
            currentTime -= Time.deltaTime;
        }
    }

    IEnumerator Countdown(int seconds)
    {
        timerStarted = true;
        
        int count = seconds;

        while (count > 0)
        {

            // display something...
            countdown.SetActive(true);
            

            yield return new WaitForSeconds(1);
            count--;
            countdownText.text = currentTime.ToString("0");
        }

        // count down is finished...
        StartGame();
    }

    void StartGame()
    {
        countdown.SetActive(false);
        timerStarted = false;
        // do something...
        GameManager.Instance.RaceStarted = true;
    }
}
