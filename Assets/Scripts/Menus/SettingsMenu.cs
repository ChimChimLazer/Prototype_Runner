using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class SettingsMenu : MonoBehaviour
{
    public GameObject mainMeu;

    private float SensX;
    private float SensY;

    private int FOV;


    // X Sensitivity 
    public GameObject SensXObject;
    private Slider SensXSlider;
    private TextMeshProUGUI SensXTextBox;

    // Y Sensitivity 
    public GameObject SensYObject;
    private Slider SensYSlider;
    private TextMeshProUGUI SensYTextBox;

    // Invert Sensitivity
    public Toggle InvertXToggle;
    public Toggle InvertYToggle;

    // FOV
    public GameObject FOVObject;
    private Slider FOVSlider;
    private TextMeshProUGUI FOVTextBox;

    private void Start()
    {
        SensXSlider = SensXObject.GetComponentInChildren<Slider>();
        SensXTextBox = SensXObject.GetComponentInChildren<TextMeshProUGUI>();
        SensYSlider = SensYObject.GetComponentInChildren<Slider>();
        SensYTextBox = SensYObject.GetComponentInChildren<TextMeshProUGUI>();

        FOVSlider = FOVObject.GetComponentInChildren<Slider>();
        FOVTextBox = FOVObject.GetComponentInChildren<TextMeshProUGUI>();
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

    public void updateFOV()
    {
        FOV = (int)FOVSlider.value;
        FOVTextBox.text = "Field of View\n" + FOV;
    }

    public void applySettings()
    {
        Debug.Log(FOV);
    }

    public void mainMenu()
    {
        mainMeu.SetActive(true); 
        gameObject.SetActive(false);
    }
}
