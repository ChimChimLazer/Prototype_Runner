using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script manages the checkpoints in the game

public class checkpointFX : MonoBehaviour
{
    public int checkpointNumber; // what checkpoint number the checkpoint is

    [SerializeField] GameObject redLight; // checkpoint off light
    [SerializeField] GameObject greenLight; // checkpoint on light

    // change light to the green light
    public void activeLight()
    {
        redLight.SetActive(false);
        greenLight.SetActive(true);
    }
}
