using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Highscores
{
    public static float[] highscores;

    public static void createHighscores(int length)
    {
        highscores = new float[length];
    }
}
