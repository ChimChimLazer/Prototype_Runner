using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject levelSelect;

    public void play()
    {
        levelSelect.SetActive(true);
        gameObject.SetActive(false);
    }

    public void options()
    {
        optionsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void exitGame()
    {
        //https://gamedevbeginner.com/how-to-quit-the-game-in-unity/#:~:text=You%20can%20quit%20a%20game,Play%20Mode%20in%20the%20editor.
        Debug.Log("Closing Game");
        Application.Quit();
    }
}
