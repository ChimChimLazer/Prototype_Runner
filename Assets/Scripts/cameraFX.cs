using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFX : MonoBehaviour
{
    private float wallRunRotation;
    private float currentFOV;

    [Header("Modifiers")]
    public float wallRunTiltSpeed;
    public float FOVIncreaseSpeed;
    [Header("References")]
    public GameObject playerCamera;
    public Camera gameCamera;

    private void Start()
    {
        wallRunTiltSpeed = wallRunTiltSpeed / 100;
        currentFOV = playerMovement.playerFOV;
    }
    void Update()
    {
        //Quaternion cameraTilt = Quaternion.Euler(transform.rotation.x, transform.rotation.y, playerMovement.wallRunCameraRotation);
        //transform.localRotation = cameraTilt;
        
        if (wallRunRotation != playerMovement.wallRunCameraRotation)
        {
            wallRunRotation = playerMovement.wallRunCameraRotation;

            Vector3 cameraTilt = new Vector3(0, 0, wallRunRotation);
            // Easing was created by using LeanTween by Dented Pixel
            // https://assetstore.unity.com/packages/tools/animation/leantween-3595#description
            LeanTween.rotateLocal(playerCamera, cameraTilt, wallRunTiltSpeed);
        }
        if (currentFOV != playerMovement.playerFOV)
        {
            currentFOV = Mathf.Lerp(currentFOV, playerMovement.playerFOV, FOVIncreaseSpeed*Time.deltaTime);

            gameCamera.fieldOfView = currentFOV;
        }
    }
}
