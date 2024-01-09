using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCombat : MonoBehaviour
{
    [Header("Health")]
    public float playerHealth;
    public float maxHealth;
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
    public playerHealthBar healthBar;

    void Start()
    {
        maxHeal();
        lastFrameHealth = playerHealth;
        regenerationCoolDown = regenerationCoolDownTime;
    }

    void Update()
    {
        healthRegeneration();
        itemPickUp();

        if (Input.GetKey(KeyCode.E))
        {
            interact();
        }
    }

    public void maxHeal()
    {
        playerHealth = maxHealth;
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
            healthBar.setHealth(playerHealth);
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

    void interact()
    {
        RaycastHit hit;
        if(Physics.Raycast(playerCameraRotation.position, playerCameraRotation.forward, out hit, interactionDistance))
        {
            if(hit.collider.tag == "Interactable Button")
            {
                interactableButton button = hit.collider.GetComponentInParent<interactableButton>();
                if (!button.pressed)
                {
                    button.press();
                }
            }
        }
    }

    public void removeHealth(float damage)
    {
        playerHealth -= damage;
        healthBar.setHealth(playerHealth);

        if (playerHealth <= 0) 
        {
            playerDie();
        }
    }

    private void playerDie() {

        // drop weapon if player has one
        if (CurrentWeapon != null)
        {
            CurrentWeapon.DropWeapon();
        }

        // Readys Respawn Script
        playerRespawning respawningScript = gameObject.GetComponentInParent<playerRespawning>();
        respawningScript.alive = false;

        // Deactives player
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Death Zone")
        {
            playerDie();
        }
    }
}
