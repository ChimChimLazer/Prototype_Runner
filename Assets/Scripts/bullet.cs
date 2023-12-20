using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float bulletSpeed;
    public Rigidbody rb;

    void FixedUpdate()
    {
        rb.AddForce(transform.forward*bulletSpeed);
    }
}
