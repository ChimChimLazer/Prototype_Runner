using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    // called on the first frame
    void Start()
    {
        Time.timeScale = 0; // pauses games
        // unlocks and shows cursor
        Cursor.lockState = CursorLockMode.Confined; 
        Cursor.visible = true;
    }

    // called every frame 
    void Update()
    {
        // reumes games when escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            resume();
        }
    }

    // resumes games when called 
    public void resume()
    {
        // resumes games
        Time.timeScale = 1;
        // unlocks and shows cursor
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible = false;
        // destroys pause menu
        Destroy(gameObject);
    }

    // restarts level
    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // loads main menu
    public void quitLevel()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
