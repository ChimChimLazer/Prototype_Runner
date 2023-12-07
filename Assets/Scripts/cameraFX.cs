using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFX : MonoBehaviour
{
    public int WallRunFOVChange;

    private float wallRunRotation;
    public Camera playerCamera;

    void Update()
    {
        if (wallRunRotation != playerMovement.wallRunCameraRotation)
        {
            wallRunRotation = playerMovement.wallRunCameraRotation;
            setTiltZ(wallRunRotation);
            if (wallRunRotation != 0)
            {
                changeFOV(WallRunFOVChange);
            } else
            {
                changeFOV(-WallRunFOVChange);
            }
            
        }
    }

    void setTiltZ(float newTilt)
    {
        Quaternion cameraTilt = Quaternion.Euler(transform.rotation.x, transform.rotation.y, newTilt);
        transform.localRotation = cameraTilt;
    }

    void changeFOV(int FOVChange)
    {
        playerCamera.fieldOfView += FOVChange;
    }
}
