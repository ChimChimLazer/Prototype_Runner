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
        Quaternion cameraTilt = Quaternion.Euler(transform.rotation.x, transform.rotation.y, playerMovement.wallRunCameraRotation);
        transform.localRotation = cameraTilt;
    }
}
