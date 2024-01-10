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
            timerText = GlobalFunctions.convertTimeToText(timer);
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
}
