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

// functionaly for the settings menu

public class SettingsMenu : MonoBehaviour
{
    public GameObject mainMeu; // main menu object

    public float SensX; // sensitivty X variable
    public float SensY; // sensitivty Y variable

    public bool invertX; // invert X variable
    public bool invertY; // invert Y variable

    public int FOV; // FOV variable


    // X Sensitivity 
    public GameObject SensXObject; // X sensitivty settings object
    private Slider SensXSlider; // X sensitivty slider
    private TextMeshProUGUI SensXTextBox; // X sensitivity text box

    // Y Sensitivity 
    public GameObject SensYObject; // Y sensitivty settings object
    private Slider SensYSlider; // Y sensitivty slider
    private TextMeshProUGUI SensYTextBox; // Y sensitivity text box

    // Invert Sensitivity
    public Toggle InvertXToggle; // Invert X Toggle
    public Toggle InvertYToggle; // Invert Y Toggle

    // FOV
    public GameObject FOVObject; // FOV settings object
    private Slider FOVSlider; // FOV slider
    private TextMeshProUGUI FOVTextBox; // FOV Textbox

    // Wipe Highscore Pop Up
    public GameObject wipeTextBoxPopUpObject; // WipeTextBox PopUp
    public LevelElement[] levels; // Level elements on level select screen

    // called on first frame
    private void Start()
    {
        // get sliders and text boxs for the settings
        SensXSlider = SensXObject.GetComponentInChildren<Slider>();
        SensXTextBox = SensXObject.GetComponentInChildren<TextMeshProUGUI>();
        SensYSlider = SensYObject.GetComponentInChildren<Slider>();
        SensYTextBox = SensYObject.GetComponentInChildren<TextMeshProUGUI>();

        FOVSlider = FOVObject.GetComponentInChildren<Slider>();
        FOVTextBox = FOVObject.GetComponentInChildren<TextMeshProUGUI>();

        // load settings file
        loadSettings();
    }

    // loads the users settings
    void loadSettings()
    {
        // Loads settings file
        userSettings data = SaveSystem.loadUserSettings();
        // Sets all values in settings to the one stored on the file
        if (data != null)
        {
            SensXSlider.value = data.SensX;
            updateSenx();
            SensYSlider.value = data.SensY;
            updateSenY();

            InvertXToggle.isOn = data.InvertX;
            InvertYToggle.isOn = data.InvertY;

            FOVSlider.value = data.FOV;
            updateFOV();
        }
    }

    // Updates X sensitivty variable when slider is changed
    public void updateSenx()
    {
        SensX = SensXSlider.value;
        SensXTextBox.text = "Horizontal Sensitivity\n" + (MathF.Round(SensX*10)/10);
    }

    // Updates Y sensitivty variable when slider is changed
    public void updateSenY()
    {
        SensY = SensYSlider.value;
        SensYTextBox.text = "Vertical Sensitivity\n" + (MathF.Round(SensY * 10) / 10);
    }

    // Updates FOV variable when slider is changed
    public void updateFOV()
    {
        FOV = (int)FOVSlider.value;
        FOVTextBox.text = "Field of View\n" + FOV;
    }

    // Saves settings to settings file
    public void applySettings()
    {
        invertX = InvertXToggle.isOn;
        invertY = InvertYToggle.isOn;

        SaveSystem.saveUserSettings(this);
    }

    // Opens main menu
    public void mainMenu()
    {
        mainMeu.SetActive(true); 
        gameObject.SetActive(false);
    }

    // opens wipe text pop up
    public void wipeTextBoxPopUp()
    {
        wipeTextBoxPopUpObject.SetActive(true);
    }

    // closes wipe text pop up
    public void wipeTextBoxNo()
    {
        wipeTextBoxPopUpObject.SetActive(false);
    }

    // deletes highscores file and closes wipe text pop up
    public void wipeTextBoxYes()
    {
        SaveSystem.deleteHighScore();
        foreach (LevelElement level in levels)
        {
            level.updateHighscores();
        }
        wipeTextBoxNo();
    }
}
