using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelFinishUI : MonoBehaviour
{
    public sceneLoader loader; // sceneLoader that is on the current scene

    [Header("Internal Fields")]
    public TextMeshProUGUI timeText; // Text box on the UI that displays the time taken to complete the level
    public TextMeshProUGUI highscoreText; // Text box on the UI that displays the highscore of the current level
    public TextMeshProUGUI levelNameText; // Text box on the UI that displays the name of the current level

    public GameObject nextLevelButtonObject; // Button that allows the user to load the next level

    private gameGUI GUI; // Game GUI that displays the timer

    // Ran on the first frame this object is created
    void Start()
    {
        GUI = loader.GUI; // Gets GUI fromt the loader

        // Updates the text box's on the UI
        levelNameText.text = SceneManager.GetActiveScene().name;
        timeText.text = ("Time : " + GUI.timerText);
        highscoreText.text = ("Highscore : " + GlobalFunctions.convertTimeToText(loader.highscores[loader.currentScene]));

        // If the current level is the last level hide the next level button
        if (loader.currentScene == loader.scenes.Length - 1)
        {
            nextLevelButtonObject.SetActive(false);
        }

        // Pause the game
        Time.timeScale = 0;
    }

    // Called when the restart button is pressed on the ui
    // This will reload the current level
    public void restartLevelButton()
    {
        loader.restartScene();
    }

    // Called when the next level button is pressed
    // This will load the next level
    public void nextLevelButton()
    {
        loader.loadNextScene();
    }

    // Called when the main menu button is pressed
    // This will load the main menu
    public void mainMenuButton() 
    {
        SceneManager.LoadScene("Main Menu");
    }
}
