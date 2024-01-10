using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script handle the doors in the game

public class door : MonoBehaviour
{
    // condition that can be used to open door
    public enum condition
    {
        ButtonInteractable,
        WeightedButton,
        DefeatedEnemys,
    }
    [Header("Condition")]
    public condition OpenCondition; // condition required to open door
    public GameObject[] conditionItems; // items used in the condition

    [Header("Opening")]
    public Vector3 openPosition; // positon of door when open
    public float openingTime; // time it takes to open and close door

    private Vector3 closedPosition; // position of door when open
    private bool doorOpen; // is door open?

    // called on first frame
    void Start()
    {
        // initalise door open
        doorOpen = false;
        // get closed position
        closedPosition = transform.position;
    }

    // called on every frame
    void Update()
    {
        // checks if door is open
        if (!doorOpen)
        {
            // witch condtion is used to open door
            switch (OpenCondition)
            {
                case condition.ButtonInteractable:
                    if (checkButtonInteractable())
                    {
                        open();
                    }
                    break;

                case condition.WeightedButton:
                    if (checkButtonWeighted())
                    {
                        open();
                    }
                    break;

                case condition.DefeatedEnemys:
                    if (checkDefeatedEnemys())
                    {
                        open();
                    }
                    break;
            }
        }  else
        {
            // if the contion is weighted but check if the button has been deactivated
            if (OpenCondition == condition.WeightedButton)
            {
                if (!checkButtonWeighted())
                {
                    close();
                }
            }
        }
    }

    // open door
    void open()
    {
        doorOpen = true;
        // use lean tween to move the door to the open position smoothy
        LeanTween.move(gameObject, openPosition, openingTime);
    }

    // close door
    void close()
    {
        doorOpen = false;
        // use lean tween to move the door to the closed position smoothy
        LeanTween.move(gameObject, closedPosition, openingTime);
    }

    // checks if all interactable buttons have been pressed
    bool checkButtonInteractable()
    {
        // loops for each button
        foreach (GameObject button in conditionItems)
        {
            interactableButton buttonInfo = button.GetComponent<interactableButton>();
            // if any but it not pressed this function will return false
            if (!buttonInfo.pressed)
            {
                return false;
            }
        }
        // if the function reaches this point without returning false all button must be pressed
        return true;
    }

    // checks if all weighted buttons have been pressed
    bool checkButtonWeighted()
    {
        // loops for each button
        foreach (GameObject button in conditionItems)
        {
            // if any but it not pressed this function will return false
            if (!button.GetComponentInChildren<weightedButton>().pressed)
            {
                return false;
            }
        }
        // if the function reaches this point without returning false all button must be pressed
        return true;
    }

    // checks if all enemys have been defeated
    bool checkDefeatedEnemys()
    {
        // loops for each enemy
        foreach (GameObject enemy in conditionItems)
        {
            // if any enemy is not null return false
            if (enemy != null)
            {
                return false;
            }
        }
        // if the function reaches this point without returning false all the enemys must havee been defeated
        return true;
    }
}
