using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelFinishUI : MonoBehaviour
{
    public sceneLoader loader;
    public void restartLevelButton()
    {
        loader.restartScene();
    }

    public void nextLevelButton()
    {
        loader.loadNextScene();
    }
}
