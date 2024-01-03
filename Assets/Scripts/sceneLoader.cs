using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLoader : MonoBehaviour
{
    public string[] scenes;

    private int currentScene;

    void Start()
    {
        currentScene = getCurrentSceneNum(SceneManager.GetActiveScene().name);
        Debug.Log(currentScene);
    }

    int getCurrentSceneNum(string sceneName)
    {
        int x = 0;
        foreach (string scene in scenes)
        {
            if (scene == sceneName)
            {
                return x;
            }
            x++;
        }
        return -1;
    }
}
