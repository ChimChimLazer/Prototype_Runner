using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLoader : MonoBehaviour
{
    public string[] scenes; // the level list
    public GameObject levelFinishMenuPrefab; // level finish screen
    public GameObject pauseMenuPrefab; // pause menu
    public gameGUI GUI; // Game GUI

    public int currentScene; // current scene number
    private bool levelFinished; // bools that check if level is finished

    public float[] highscores; // list of highscores

    private GameObject currentPauseMenu; // pause menu that is openned

    // called on first frame
    void Start()
    {
        // get current scene number
        currentScene = getCurrentSceneNum(SceneManager.GetActiveScene().name);
        // initalise level finished
        levelFinished = false;
        //load highscores list
        loadHighscore();
    }

    // called every frame
    private void Update()
    {
        // opens pause menu when escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape) && currentPauseMenu == null)
        {
            currentPauseMenu = Instantiate(pauseMenuPrefab);
        }
    }

    // loads the highscore list
    void loadHighscore()
    {
        highscoreData data = SaveSystem.loadHighscore();
        if (data != null)
        {
            highscores = new float[data.highscores.Length];
            int x = 0;
            foreach (float item in data.highscores)
            {
                highscores[x] = item;
                x++;
            }
        }
        else
        {
            // creates an empty highscore list if one does not exist

            //Debug.Log("new file created");
            highscores = new float[scenes.Length];
            saveHighscore();
        }
    }

    // saves highscore list
    void saveHighscore()
    {
        SaveSystem.saveHighscore(this);
    }

    // Uses's a linear search to find the sceneName in the scene list and then returns that index number
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

    // loads the next scene in the list
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

    // restarts current scene
    public void restartScene()
    {
        SceneManager.LoadScene(currentScene);
    }

    // Opens end level GUI when finish line is collided with by the player
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!levelFinished)
            {
                GUI.stopTimer();
                levelFinished = true;

                // overrights current levels highscore with the time the user completed the level if the user completed it faster then the highscore
                if (GUI.timer < highscores[currentScene] || highscores[currentScene] == 0)
                {
                    highscores[currentScene] = GUI.timer;
                    saveHighscore();
                }

                // unclocks and shows the cursor
                Cursor.lockState = CursorLockMode.Confined; Cursor.visible = true;

                // opens level finished menu
                GameObject levelFinishMenu = Instantiate(levelFinishMenuPrefab);
                levelFinishUI levelFinish = levelFinishMenu.GetComponent<levelFinishUI>();
                levelFinish.loader = this;
            }
        }
    }
}
