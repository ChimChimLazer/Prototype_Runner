using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public float despawnTime;

    private float timeAlive;
    private Rigidbody rb;

    private void Start()
    {
        timeAlive = 0;
        rb = GetComponent<Rigidbody>();

        Vector3 bulletForce = transform.forward * speed;

        rb.AddForce(bulletForce, ForceMode.Impulse);
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;

        if (timeAlive >= despawnTime)
        {
            Destroy(gameObject);
        }
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
