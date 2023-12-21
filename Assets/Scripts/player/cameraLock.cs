using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraLock : MonoBehaviour
{

    public Transform playerTransform;

    void Update()
    {
        transform.position = playerTransform.position;
    }
}
