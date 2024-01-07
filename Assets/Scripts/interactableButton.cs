using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactableButton : MonoBehaviour
{
    public bool pressed;
    [SerializeField] float pressedDepth;
    [SerializeField] float pressingDepth;
    [SerializeField] GameObject button;

    private float pressedY;
    private float unpressedY;
    private float pressingY;

    private void Start()
    {
        pressed = false;
        unpressedY = button.transform.localPosition.y;
        pressedY = unpressedY - pressedDepth;
        pressingY = unpressedY - pressingDepth;
    }
    private void Update()
    {
        if (button.transform.localPosition.y == pressingY)
        {
            pressed = true;
            LeanTween.moveLocalY(button, pressedY, 0.125f);
        }
    }

    public void press()
    {
        LeanTween.moveLocalY(button, pressingY, 0.25f);
    }
}
