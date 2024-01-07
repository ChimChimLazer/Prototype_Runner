using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public enum condition
    {
        ButtonInteractable,
        WeightedButton,
        DefeatedEnemys,
    }
    [Header("Condition")]
    public condition OpenCondition;
    public GameObject[] conditionItems;

    [Header("Opening")]
    public Vector3 openPosition;
    public float openingTime;

    private Vector3 closedPosition;
    private bool doorOpen;

    void Start()
    {
        doorOpen = false;
        closedPosition = transform.position;
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
            if (OpenCondition == condition.WeightedButton)
            {
                if (!checkButtonWeighted())
                {
                    close();
                }
            }
        }
    }

    void open()
    {
        doorOpen = true;
        LeanTween.move(gameObject, openPosition, openingTime);
    }

    void close()
    {
        doorOpen = false;
        LeanTween.move(gameObject, closedPosition, openingTime);
    }

    bool checkButtonInteractable()
    {
        foreach (GameObject button in conditionItems)
        {
            interactableButton buttonInfo = button.GetComponent<interactableButton>();
            if (!buttonInfo.pressed)
            {
                return false;
            }
        }
        return true;
    }

    bool checkButtonWeighted()
    {
        foreach (GameObject button in conditionItems)
        {
            if (!button.GetComponentInChildren<weightedButton>().pressed)
            {
                return false;
            }
        }
        return true;
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
}
