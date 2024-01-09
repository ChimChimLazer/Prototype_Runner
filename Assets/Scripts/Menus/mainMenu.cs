using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenu : MonoBehaviour
{
    public void play()
    {

    }

    public void options()
    {

    }

    public void exitGame()
    {
        //https://gamedevbeginner.com/how-to-quit-the-game-in-unity/#:~:text=You%20can%20quit%20a%20game,Play%20Mode%20in%20the%20editor.
        Debug.Log("Closing Game");
        Application.Quit();
    }
}
