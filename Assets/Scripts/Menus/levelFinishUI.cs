using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class levelFinishUI : MonoBehaviour
{
    public sceneLoader loader;

    [Header("Internal Fields")]
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI highscoreText;

    private gameGUI GUI;

    void Start()
    {
        GUI = loader.GUI;

        timeText.text = ("Time : " + GUI.timerText);
    }

    public void restartLevelButton()
    {
        loader.restartScene();
    }

    public void nextLevelButton()
    {
        loader.loadNextScene();
    }
}
