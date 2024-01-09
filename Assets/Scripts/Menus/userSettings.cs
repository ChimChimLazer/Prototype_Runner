using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class userSettings
{
    public float SensX;
    public float SensY;

    public bool InvertX;
    public bool InvertY;

    public int FOV;

    public userSettings (SettingsMenu settings)
    {
        SensX = settings.SensX;
        SensY = settings.SensY;

        InvertX = settings.invertX;
        InvertY = settings.invertY;

        FOV = settings.FOV;
    }
}
