using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelElement : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Highscore;
    public int levelNumber;

    private void Start()
    {
        updateHighscores();
    }

    public void updateHighscores()
    {
        highscoreData data = SaveSystem.loadHighscore();
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

    public void OnButtonClick()
    {
        SceneManager.LoadScene(Name.text);
    }
}
