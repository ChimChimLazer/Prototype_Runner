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

    public float SensX;
    public float SensY;

    public bool invertX;
    public bool invertY;

    public int FOV;


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

        loadSettings();
    }

    void loadSettings()
    {
        userSettings data = SaveSystem.loadUserSettings();
        if (data != null)
        {
            SensXSlider.value = data.SensX;
            SensYSlider.value = data.SensY;

            InvertXToggle.isOn = data.InvertX;
            InvertYToggle.isOn = data.InvertY;

            FOVSlider.value = data.FOV;
        }
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
        invertX = InvertXToggle.isOn;
        invertY = InvertYToggle.isOn;

        SaveSystem.saveUserSettings(this);
    }

    public void mainMenu()
    {
        mainMeu.SetActive(true); 
        gameObject.SetActive(false);
    }
}
