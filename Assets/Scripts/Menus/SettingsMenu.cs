using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class SettingsMenu : MonoBehaviour
{
    public GameObject mainMeu;

    private float SensX;
    private float SensY;

    private bool invertX;
    private bool invertY;

    private float FOV;


    public GameObject SensXObject;
    private Slider SensXSlider;
    private TextMeshProUGUI SensXTextBox;

    public GameObject SensYObject;
    private Slider SensYSlider;
    private TextMeshProUGUI SensYTextBox;


    private void Start()
    {
        SensXSlider = SensXObject.GetComponentInChildren<Slider>();
        SensXTextBox = SensXObject.GetComponentInChildren<TextMeshProUGUI>();
        SensYSlider = SensYObject.GetComponentInChildren<Slider>();
        SensYTextBox = SensYObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void updateSenx()
    {
        SensX = SensXSlider.value;
        SensXTextBox.text = "Horizontal Sensitivity\n" + (MathF.Round(SensX*10)/10);
    }

    public void updateSenY()
    {
        SensY = SensYSlider.value;
        SensYTextBox.text = "Vertical Sensitivity\n" + (MathF.Round(SensY * 10) / 10);
    }

    public void applySettings()
    {
        Debug.Log(SensX);
        Debug.Log(SensY);

        Debug.Log(invertX);
        Debug.Log(invertY);

        Debug.Log(FOV);
    }

    public void mainMenu()
    {
        mainMeu.SetActive(true); 
        gameObject.SetActive(false);
    }
}
