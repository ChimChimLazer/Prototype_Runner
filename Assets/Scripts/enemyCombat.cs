using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCombat : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] float maxHealth;
    private float health;

    private void Start()
    {
        health = maxHealth;
    }

    public void removeHealth(float damange)
    {
        health -= damange;
        Debug.Log(gameObject + " Health = " + health);

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