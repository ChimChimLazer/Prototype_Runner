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
            Debug.Log(levelNumber);
            float highscoreNum = data.highscores[levelNumber];
            Highscore.text = convertTimeToText(highscoreNum);
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

    public string convertTimeToText(float time)
    {
        int milliseconds = Mathf.RoundToInt((time % 1) * 1000);
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
