using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script manages each one of the level elements in the level select screen
public class LevelElement : MonoBehaviour
{
    public TextMeshProUGUI Name; // Field that displays the name of the level
    public TextMeshProUGUI Highscore; // Field that displays the best time the player has on the level
    public int levelNumber;

    // Runs once on the first frame the object is created
    private void Start()
    {
        updateHighscores();
    }

    // Updates the highscore field to be the high score of the level that is shown on the level element
    public void updateHighscores()
    {
        highscoreData data = SaveSystem.loadHighscore(); // Loads the users highscores
        // If highscore data is found the highscore from the level on the level element and display it in the text box
        // If there is no highscore data "0:00:00" will be displayed as the user will not have played the level
        if (data != null)
        {
            float highscoreNum = data.highscores[levelNumber];
            Highscore.text = GlobalFunctions.convertTimeToText(highscoreNum);
        }
        else
        {
            Highscore.text = "0:00:000";
        }
    }

    // This function is ran when the button is clicked.
    // It will load the corrisponding level when pressed
    public void OnButtonClick()
    {
        SceneManager.LoadScene(Name.text);
    }
}
