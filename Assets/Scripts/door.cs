using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public enum condition
    {
        ButtonInteractable,
        DefeatedEnemys,
    }
    [Header("Condition")]
    public condition OpenCondition;
    public GameObject[] conditionItems;

    [Header("Position")]
    public Vector3 openPosition;
    private Vector3 closedPosition;

    private bool doorOpen;

    void Start()
    {
        closedPosition = transform.position;
        doorOpen = false;
    }

    void Update()
    {
        if (!doorOpen)
        {
            switch (OpenCondition)
            {
                case condition.ButtonInteractable:
                    if (checkButtonInteractable())
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
        }
    }

    void open()
    {
        doorOpen = true;
        Debug.Log("Door is open");
    }

    bool checkDefeatedEnemys()
    {
        foreach (GameObject enemy in conditionItems)
        {
            if (enemy != null)
            {
                return false;
            }
        }
        return true;
    }

    bool checkButtonInteractable()
    {
        foreach(GameObject button in conditionItems)
        {
            interactableButton buttonInfo = button.GetComponent<interactableButton>();
            if (!buttonInfo.pressed)
            {
                return false;
            }
        }
        return true;
    }
}
