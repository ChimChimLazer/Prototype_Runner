using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFX : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Quaternion cameraTilt = Quaternion.Euler(transform.rotation.x, transform.rotation.y, playerMovement.wallRunCameraRotation);
        transform.localRotation = cameraTilt;
    }
}
