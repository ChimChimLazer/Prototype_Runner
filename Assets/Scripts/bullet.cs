using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class manages the bullets in the game

public class bullet : MonoBehaviour
{
    public float speed; // bullet speed
    public float damage; // bullet damage
    public float despawnTime; // how long it takes for bullets to despawn if they haven't already hit a wall

    private float timeAlive; // how long the bullet has been alive for 
    private Rigidbody rb; // bullets rigidbody

    // calls on the first frame the bullet has been created
    private void Start()
    {
        // initalise timeAlive
        timeAlive = 0;
        // get Rigidbody
        rb = GetComponent<Rigidbody>();

        // add force to the bullet
        Vector3 bulletForce = transform.forward * speed;
        rb.AddForce(bulletForce, ForceMode.Impulse);
    }

    // calls every frame
    private void Update()
    {
        // updates time alive
        timeAlive += Time.deltaTime;

        // destroys bullet if its been alive for too long
        if (timeAlive >= despawnTime)
        {
            Destroy(gameObject);
        }
    }

    // Calls when the bullet collides with an object
    private void OnCollisionEnter(Collision collision)
    {
        // checks that it didn't close with an enemy
        if (collision.collider.tag != "Enemy")
        {
            // checks if the bullet hit the player
            if(collision.collider.tag == "Player")
            {
                // get player combat script
                playerCombat playerHit = collision.gameObject.GetComponentInParent<playerCombat>();
                // damage player
                playerHit.removeHealth(damage);
            }
            // destroy bullet
            Destroy(gameObject);
        }
    }
}
