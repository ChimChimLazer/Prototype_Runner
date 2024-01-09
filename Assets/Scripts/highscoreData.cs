using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class highscoreData
{
    public float[] highscores;

    public highscoreData (sceneLoader loader)
    {
        highscores = new float[loader.highscores.Length];
        highscores = loader.highscores;
    }
}
