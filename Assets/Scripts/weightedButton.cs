using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class weightedButton : MonoBehaviour
{
    public bool pressed; // button pressed?
    void Start()
    {
        // initalises pressed
        pressed = false;
    }

    // If a collider is colliding with the button the pressed = true
    void OnCollisionStay(Collision collision)
    {
        pressed = true;
    }

    // When colliders stop colliding with the button pressed = false
    void OnCollisionExit(Collision collision)
    {
        pressed = false;
    }
}
