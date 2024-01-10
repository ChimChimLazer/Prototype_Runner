using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sets camera position to the players transform
public class cameraLock : MonoBehaviour
{

    public Transform playerTransform; // players transform

    void Update()
    {
        // sets camera position to the players transform
        transform.position = playerTransform.position;
    }
}
