using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject optionsMenu;

    private float clickDelay;
    private float clickDelayTime;
    private bool delayTimeCheck;
    private void Start()
    {
        Time.timeScale = 1;
        clickDelayTime = 2f;
        clickDelay = 0;
        delayTimeCheck = false;
    }
    private void Update()
    {
        inputDelay();
    }

    public void play()
    {
        if (delayTimeCheck)
        {
            SceneManager.LoadScene("Level 1");
        }
    }

    public void options()
    {
        if (delayTimeCheck)
        {
            optionsMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void exitGame()
    {
        if (delayTimeCheck)
        {
            //https://gamedevbeginner.com/how-to-quit-the-game-in-unity/#:~:text=You%20can%20quit%20a%20game,Play%20Mode%20in%20the%20editor.
            Debug.Log("Closing Game");
            Application.Quit();
        }
    }

    private void inputDelay()
    {
        if(clickDelay < clickDelayTime)
        {
            delayTimeCheck = true;
        }
        else
        {
            clickDelay += Time.deltaTime;
        }
    }
}
