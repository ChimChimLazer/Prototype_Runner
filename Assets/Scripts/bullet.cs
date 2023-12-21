using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        Vector3 bulletForce = transform.forward * 20;

        rb.AddForce(bulletForce, ForceMode.Impulse);
    }
}
