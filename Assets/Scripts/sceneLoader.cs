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

    void loadNextScene()
    {
        int nextSceneNumber = currentScene + 1;

        if (nextSceneNumber < scenes.Length)
        {
            string nextScene = scenes[nextSceneNumber];
            SceneManager.LoadScene(nextScene);

        } else
        {
            Debug.Log("No more Scenes");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            loadNextScene();
        }
    }
}
