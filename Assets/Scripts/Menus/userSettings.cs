using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class will store the users setting witch will be stored as a file
// Makes the class Serializable
[System.Serializable]
public class userSettings
{
    public float SensX;
    public float SensY;

    public bool InvertX;
    public bool InvertY;

    public int FOV;

    // create using setting function
    public userSettings (SettingsMenu settings)
    {
        SensX = settings.SensX;
        SensY = settings.SensY;

        InvertX = settings.invertX;
        InvertY = settings.invertY;

        FOV = settings.FOV;
    }

    // create using individual variables
    public userSettings(float sensX, float sensY, bool invertX, bool invertY, int fOV)
    {
        SensX = sensX;
        SensY = sensY;
        InvertX = invertX;
        InvertY = invertY;
        FOV = fOV;
    }
}
