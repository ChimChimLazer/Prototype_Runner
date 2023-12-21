using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float exitSpeed;
    public float damage;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        Vector3 bulletForce = transform.forward * exitSpeed;

        rb.AddForce(bulletForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Enemy")
        {
            if(collision.collider.tag == "Player")
            {
                playerCombat playerHit = collision.gameObject.GetComponentInParent<playerCombat>();
                playerHit.removeHealth(damage);
            }
            Destroy(gameObject);
        }
    }
}
