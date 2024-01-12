using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject optionsMenu; // options menu object
    public GameObject levelSelect; // Level select menu object

    // opens level select screen
    public void play()
    {
        levelSelect.SetActive(true);
        gameObject.SetActive(false);
    }

    // opens options menu
    public void options()
    {
        optionsMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    // closes games
    public void exitGame()
    {
        //https://gamedevbeginner.com/how-to-quit-the-game-in-unity/#:~:text=You%20can%20quit%20a%20game,Play%20Mode%20in%20the%20editor.
        Application.Quit();
        Debug.Log("Closed Game");
    }
}
