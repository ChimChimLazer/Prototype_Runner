using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCombat : MonoBehaviour
{
    [Header("Health")]
    public float playerHealth;
    [SerializeField] float maxHealth;
    private float lastFrameHealth;

    [Header("Health Regeneration")]
    public float regenerationRate;
    public float regenerationCoolDownTime;
    private float regenerationCoolDown;

    [Header("Interaction")]
    public float interactionDistance;

    [Header("Weapons")]
    public weapon CurrentWeapon;

    [Header("References")]
    public Transform playerCameraRotation;

    void Start()
    {
        playerHealth = maxHealth;
        lastFrameHealth = playerHealth;
        regenerationCoolDown = regenerationCoolDownTime;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            playerHealth -= 10;
        }

        healthRegeneration();
        itemPickUp();
    }
    void healthRegeneration()
    {
        // When player loses health
        if (lastFrameHealth > playerHealth) 
        {
            regenerationCoolDown = 0;

        } else if (regenerationCoolDown < regenerationCoolDownTime)
        {
            
            regenerationCoolDown += Time.deltaTime;

        } if (regenerationCoolDown >= regenerationCoolDownTime && playerHealth<maxHealth)
        {
            
            playerHealth += Time.deltaTime * regenerationRate;

            if (playerHealth > maxHealth)
            {
                playerHealth = maxHealth;
            }
        }
        lastFrameHealth = playerHealth;
    }

    void itemPickUp()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            if(Physics.Raycast(playerCameraRotation.position, playerCameraRotation.forward, out hit, interactionDistance))
            {
                if (hit.collider.tag == "Weapon")
                {
                    CurrentWeapon = hit.collider.gameObject.GetComponent<weapon>();

                    CurrentWeapon.pickUpWeapon(playerCameraRotation);
                }
            }
        } else if (Input.GetKeyDown(KeyCode.Q) && CurrentWeapon != null)
        {
            CurrentWeapon.DropWeapon();
        }
    }
}
