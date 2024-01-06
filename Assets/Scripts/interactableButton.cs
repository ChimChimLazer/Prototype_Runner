using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactableButton : MonoBehaviour
{
    public bool pressed;
    [SerializeField] Vector3 pressedDepth;

    private void Start()
    {
        pressed = false;
    }

    public void press()
    {
        pressed = true;
        Debug.Log("Button Pressed");
    }
}
