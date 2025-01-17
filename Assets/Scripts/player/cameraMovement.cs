using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script manages the players camera movement
// https://medium.com/@mikeyoung_97230/creating-a-simple-camera-controller-in-unity3d-using-c-ec1a79584687
// was used when creating this script
public class cameraMovement : MonoBehaviour
{
    public float sensitivityX; // players X sensitivity
    public float sensitivityY;// players Y sensitivity

    // Start is called before the first frame update
    void Start()
    {
        // Hides cursor and locks it to the centre of the screen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Load Settings from file
        loadSettings();
    }

    // Update is called once per frame
    
    void Update()
    {

        // Gets players mouseX and Y inputs
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // move players camera
        transform.eulerAngles += new Vector3(-mouseY * sensitivityY, mouseX * sensitivityX, 0);
    }

    // loads the players settings
    void loadSettings()
    {
        userSettings data = SaveSystem.loadUserSettings();

        // Apply Sensitivity Settings
        sensitivityX = data.SensX;
        sensitivityY = data.SensY;

        // Invert Camera Movement if setting is enabled
        if (data.InvertX)
        {
            sensitivityX = sensitivityX * -1;
        }
        if (data.InvertY)
        {
            sensitivityY = sensitivityY * -1;
        }
    }
}
