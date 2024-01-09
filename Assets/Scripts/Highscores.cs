using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Highscores
{
    public static float[] highscores;
    public static string[] highscoresText;

    public static void createHighscores(int length)
    {
        if(highscores == null)
        {
            highscores = new float[length];
            highscoresText = new string[length];
        }
    }
}
