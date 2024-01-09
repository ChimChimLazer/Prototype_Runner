using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static System.Net.WebRequestMethods;

public class gameGUI : MonoBehaviour
{
    //https://forum.unity.com/threads/changing-textmeshpro-text-from-ui-via-script.462250/
    public TextMeshProUGUI gameTimer;
    public float timer;
    public string timerText;
    private bool timerStarted;

    void Start()
    {
        timer = 0;
        timerStarted = false;
    }

    void Update()
    {
        if (timerStarted)
        {
            timer += Time.deltaTime;
            timerText = convertTimeToText(timer);
            gameTimer.text = timerText;
        }
    }

    public void startTimer()
    {
        timerStarted = true;
    }

    public void stopTimer()
    {
        timerStarted = false;
    }

    public string convertTimeToText(float time)
    {
        int milliseconds = Mathf.RoundToInt((timer % 1) * 1000);
        if (milliseconds > 999)
        {
            milliseconds = 0;
        }
        int seconds = Mathf.RoundToInt((time - milliseconds / 1000) % 60);
        if (seconds > 59)
        {
            seconds = 0;
        }
        int minutes = Mathf.RoundToInt((time - milliseconds / 1000 - seconds) / 60);

        // https://learn.microsoft.com/en-us/dotnet/standard/base-types/how-to-pad-a-number-with-leading-zeros?redirectedfrom=MSDN
        return (minutes + ":" + seconds.ToString("D2") + ":" + milliseconds.ToString("D3"));
    }
}
