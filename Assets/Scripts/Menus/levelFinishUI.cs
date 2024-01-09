using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelFinishUI : MonoBehaviour
{
    public sceneLoader loader;

    [Header("Internal Fields")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI levelNameText;

    private gameGUI GUI;

    void Start()
    {
        GUI = loader.GUI;

        // Update text
        levelNameText.text = SceneManager.GetActiveScene().name;
        timeText.text = ("Time : " + GUI.timerText);
        highscoreText.text = ("Highscore : " + Highscores.highscoresText[loader.currentScene]);

        Time.timeScale = 0;
    }

    public void restartLevelButton()
    {
        loader.restartScene();
    }

    public void nextLevelButton()
    {
        loader.loadNextScene();
    }

    public void mainMenuButton() 
    {
        Debug.Log("Main Menu");
    }
}
