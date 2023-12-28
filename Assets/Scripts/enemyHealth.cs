using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float maxHealth;
    private float health;

    private void Start()
    {
        maxHeal();
    }

    public void maxHeal()
    {
        health = maxHealth;
    }

    public void removeHealth(float damange)
    {
        health -= damange;

        if (health <= 0)
        {
            die();
        }
    }

    private void die()
    {
        Destroy(gameObject);
    }
}
