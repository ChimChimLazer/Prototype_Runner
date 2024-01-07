using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gameGUI : MonoBehaviour
{
    //https://forum.unity.com/threads/changing-textmeshpro-text-from-ui-via-script.462250/
    [SerializeField] TextMeshProUGUI gameTimer;
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
            gameTimer.text = timer.ToString();
        }
    }

    public void startTimer()
    {
        timerStarted = true;
    }

    private string convertTimeToText(float time)
    {
        return "";
    }
}
