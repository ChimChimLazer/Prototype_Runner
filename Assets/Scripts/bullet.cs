using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float exitSpeed;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        Vector3 bulletForce = transform.forward * exitSpeed;

        rb.AddForce(bulletForce, ForceMode.Impulse);
    }
}
