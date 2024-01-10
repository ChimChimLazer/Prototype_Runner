using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles camera effects
// E.g. rotation and FOV changes
public class cameraFX : MonoBehaviour
{
    private float wallRunRotation; // players current wall run roation
    private float currentFOV; // players current FOV

    [Header("Modifiers")]
    public float wallRunTiltSpeed; // camera tilt speed
    public float FOVIncreaseSpeed; // FOV increase speed
    [Header("References")]
    public GameObject playerCamera; // players camera object
    public Camera gameCamera; // games camera

    // called on the first frame
    private void Start()
    {
        // initalise wallRunTiltSpeed and currentFOV
        wallRunTiltSpeed = wallRunTiltSpeed / 100;
        currentFOV = playerMovement.playerFOV;
        // loads FOV settings
        loadSettings();
    }
    // Called every frame
    void Update()
    {
        // when wallRunRotation is not equal to the rotation of the camera
        if (wallRunRotation != playerMovement.wallRunCameraRotation)
        {
            wallRunRotation = playerMovement.wallRunCameraRotation;

            // eases the camera to the new rotation
            // Easing was created by using LeanTween by Dented Pixel
            // https://assetstore.unity.com/packages/tools/animation/leantween-3595#description
            Vector3 cameraTilt = new Vector3(0, 0, wallRunRotation);
            LeanTween.rotateLocal(playerCamera, cameraTilt, wallRunTiltSpeed);
        }
        // if the current FOV does not equal the players FOV
        if (currentFOV != playerMovement.playerFOV)
        {
            // ease the current FOV to slowy change to the target FOV
            currentFOV = Mathf.Lerp(currentFOV, playerMovement.playerFOV, FOVIncreaseSpeed*Time.deltaTime);

            gameCamera.fieldOfView = currentFOV;
        }
    }

    // loads FOV Settings
    void loadSettings()
    {
        userSettings data = SaveSystem.loadUserSettings();
        
        gameCamera.fieldOfView = data.FOV;
        
    }
}
