using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This object is used to store highscores for the game

// Makes objects Serializable
[System.Serializable]
public class highscoreData
{
    public float[] highscores; // highstores array

    // set highscores array
    public highscoreData (sceneLoader loader)
    {
        highscores = new float[loader.highscores.Length];
        highscores = loader.highscores;
    }
}
