using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static System.Net.WebRequestMethods;

public class gameGUI : MonoBehaviour
{
    public TextMeshProUGUI gameTimer; // Text box that displays the game timer.
    public float timer; // Game timer, this will count up when the user is playing the level.
    public string timerText; // String that stores a formatted version of the timer variable.
    private bool timerStarted; // Bool that checks if the timer should be started.

    // Called on the  first frame 
    void Start()
    {
        // Initilise timer and timerStarted variables
        timer = 0; 
        timerStarted = false;
    }

    // Called on every frame
    // Increases the timer by time.delta and updates the timer text box with the formatted version of the new time
    void Update()
    {
        if (timerStarted)
        {
            timer += Time.deltaTime;
            timerText = GlobalFunctions.convertTimeToText(timer);
            gameTimer.text = timerText;
        }
    }

    // Starts timer (called by the playerMovement script)
    public void startTimer()
    {
        timerStarted = true;
    }

    //Stops timer (called by the scene loader script)
    public void stopTimer()
    {
        timerStarted = false;
    }
}
