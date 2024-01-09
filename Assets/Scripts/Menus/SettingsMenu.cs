using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject mainMeu;

    public void mainMenu()
    {
        mainMeu.SetActive(true); 
        gameObject.SetActive(false);
    }

    public void applySettings()
    {

    }
}
