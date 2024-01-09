using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLoader : MonoBehaviour
{
    public string[] scenes;
    public GameObject levelFinishMenuPrefab;
    public gameGUI GUI;

    public int currentScene;
    private bool levelFinished;

    public float[] highscores;

    void Start()
    {
        currentScene = getCurrentSceneNum(SceneManager.GetActiveScene().name);
        levelFinished = false;
        loadHighscore();
    }

    void loadHighscore()
    {
        highscoreData data = SaveSystem.loadHighscore();
        if (data != null )
        {
            highscores = new float[data.highscores.Length];
            data.highscores = highscores;
        }
        else
        {
            highscores = new float[scenes.Length];
            saveHighscore();
        }
    }

    void saveHighscore()
    {
        SaveSystem.saveHighscore(this);
    }

    int getCurrentSceneNum(string sceneName)
    {
        int x = 0;
        foreach (string scene in scenes)
        {
            if (scene == sceneName)
            {
                return x;
            }
            x++;
        }
        return -1;
    }

    public void loadNextScene()
    {
        int nextSceneNumber = currentScene + 1;

        if (nextSceneNumber < scenes.Length)
        {
            string nextScene = scenes[nextSceneNumber];
            SceneManager.LoadScene(nextScene);

        } else
        {
            Debug.Log("No more Scenes");
        }
    }

    public void restartScene()
    {
        SceneManager.LoadScene(currentScene);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!levelFinished)
            {
                GUI.stopTimer();
                levelFinished = true;

                if (GUI.timer < highscores[currentScene] || highscores[currentScene] == 0)
                {
                    highscores[currentScene] = GUI.timer;
                }

                Cursor.lockState = CursorLockMode.Confined; Cursor.visible = true;
                GameObject levelFinishMenu = Instantiate(levelFinishMenuPrefab);
                levelFinishUI levelFinish = levelFinishMenu.GetComponent<levelFinishUI>();
                levelFinish.loader = this;
            }
        }
    }
}
