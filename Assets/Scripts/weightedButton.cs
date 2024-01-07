using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class weightedButton : MonoBehaviour
{
    public bool pressed;
    void Start()
    {
        pressed = false;
    }

    void OnCollisionStay(Collision collision)
    {
        pressed = true;
    }

    void OnCollisionExit(Collision collision)
    {
        pressed = false;
    }
}