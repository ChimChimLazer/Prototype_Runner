using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float bulletSpeed;
    public Rigidbody rb;

    private float bulletUptime;

    private void Update()
    {
        if (bulletUptime >= 5)
        {
            Destroy(gameObject);
        }
        bulletUptime += Time.deltaTime;
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward*bulletSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            
            Destroy(gameObject);
        }
    }
}
