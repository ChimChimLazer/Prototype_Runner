using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpointFX : MonoBehaviour
{
    public int checkpointNumber;

    [SerializeField] GameObject redLight;
    [SerializeField] GameObject greenLight;

    public void activeLight()
    {
        redLight.SetActive(false);
        greenLight.SetActive(true);
    }
}
