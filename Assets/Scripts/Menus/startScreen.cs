using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This function controlls the functionality of the start screen
public class startScreen : MonoBehaviour
{
    public GameObject mainMenu; // main menu gameobject

    // called on the first frame
    private void Start()
    {
        userSettings data = SaveSystem.loadUserSettings(); // loads the users settings 
        if (data == null) // if the user does not have settings create a new default settings file
        {
            SaveSystem.createDefaultSetting();
        }
    }
    // called every frame
    private void Update()
    {
        // if the user presses any key open the main menu
        if (Input.anyKeyDown) 
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
