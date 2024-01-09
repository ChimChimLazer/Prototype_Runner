using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startScreen : MonoBehaviour
{
    public GameObject mainMenu;
    private void Update()
    {
        if (Input.anyKeyDown) 
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
