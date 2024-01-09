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

    public GameObject nextLevelButtonObject;

    private gameGUI GUI;

    void Start()
    {
        GUI = loader.GUI;

        // Update text
        levelNameText.text = SceneManager.GetActiveScene().name;
        timeText.text = ("Time : " + GUI.timerText);
        highscoreText.text = ("Highscore : " + GUI.convertTimeToText(loader.highscores[loader.currentScene]));

        if (loader.currentScene == loader.scenes.Length - 1)
        {
            nextLevelButtonObject.SetActive(false);
        }

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
