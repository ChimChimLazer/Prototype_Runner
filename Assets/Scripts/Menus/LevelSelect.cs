using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public GameObject mainMenu; // main menu object

    // opens main menu when back button is pressed
    public void BackButton()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
