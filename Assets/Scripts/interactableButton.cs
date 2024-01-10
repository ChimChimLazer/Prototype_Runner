using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script handles the interactable button in the game

public class interactableButton : MonoBehaviour
{
    public bool pressed; // is the button pressed
    [SerializeField] float pressedDepth; // how far down the button goes when pressed
    [SerializeField] float pressingDepth; // how far down the button goes when pressing
    [SerializeField] GameObject button; // button

    private float pressedY; // postion of the button when pressed
    private float unpressedY; // postion of the button when not pressed
    private float pressingY; // position of the button when being pressed

    // called on first frame
    private void Start()
    {
        // pressed
        pressed = false;

        //workout unpressedY, pressedY and pressingY
        unpressedY = button.transform.localPosition.y;
        pressedY = unpressedY - pressedDepth;
        pressingY = unpressedY - pressingDepth;
    }

    // called every frame
    private void Update()
    {
        // move to pressed Y when button Y position reachs pressing Y
        if (button.transform.localPosition.y == pressingY)
        {
            pressed = true;
            LeanTween.moveLocalY(button, pressedY, 0.125f);
        }
    }

    // move button to pressing Y
    public void press()
    {
        LeanTween.moveLocalY(button, pressingY, 0.25f);
    }
}
