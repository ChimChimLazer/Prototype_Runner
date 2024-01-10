using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            resume();
        }
    }

    public void resume()
    {
        Time.timeScale = 1;
        Cursor.lockState= CursorLockMode.Locked;
        Cursor.visible = false;
        Destroy(gameObject);
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quitLevel()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
