using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float maxHealth; // enemy max health
    private float health; // current enemy health

    // called on the first frame
    private void Start()
    {
        maxHeal();
    }

    // set health to maxHealth
    public void maxHeal()
    {
        health = maxHealth;
    }

    // subtracts number from health and called die() if health is less then or equal to 0
    public void removeHealth(float damange)
    {
        health -= damange;

        if (health <= 0)
        {
            die();
        }
    }

    // destory enemy
    private void die()
    {
        Destroy(gameObject);
    }
}
