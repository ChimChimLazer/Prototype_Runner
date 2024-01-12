using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCombat : MonoBehaviour
{
    [Header("Health")]
    public float playerHealth; // players current health
    public float maxHealth; // players max health
    private float lastFrameHealth; // the health of the player on the last frame

    [Header("Health Regeneration")]
    public float regenerationRate; // rate of health regenaration
    public float regenerationCoolDownTime; // cooldown after taking damange that regeneration starts
    private float regenerationCoolDown; // timer for the regen cooldown

    [Header("Interaction")]
    public float interactionDistance; // distance were the user can interact with somthing

    [Header("Weapons")]
    public weapon CurrentWeapon; // weapon the user is currently using

    [Header("References")]
    public Transform playerCameraRotation; // players transform, to be used to get the players rotation
    public playerHealthBar healthBar; // players health bar

    // Called on first frame
    void Start()
    {
        maxHeal(); // heals player to max health

        // initalises last frame health and regen cooldown
        lastFrameHealth = playerHealth;
        regenerationCoolDown = regenerationCoolDownTime;
    }

    // called every frame
    void Update()
    {
        // handles health regeneration
        healthRegeneration();
        // handles item picking up
        itemPickUp();

        // interacts when e is pressed
        if (Input.GetKey(KeyCode.E))
        {
            interact();
        }
    }

    // sets player health to max health
    public void maxHeal()
    {
        playerHealth = maxHealth;
    }

    // Handles health regeneration
    void healthRegeneration()
    {
        // When player loses health
        if (lastFrameHealth > playerHealth) 
        {
            regenerationCoolDown = 0;

        } else if (regenerationCoolDown < regenerationCoolDownTime)
        {
            // increases healthcool down 
            regenerationCoolDown += Time.deltaTime;

        } if (regenerationCoolDown >= regenerationCoolDownTime && playerHealth<maxHealth)
        {
            // regenerate health
            playerHealth += Time.deltaTime * regenerationRate;

            if (playerHealth > maxHealth)
            {
                playerHealth = maxHealth;
            }
            // updates health bar
            healthBar.setHealth(playerHealth);
        }
        // readys lastFrameHealth for next frame
        lastFrameHealth = playerHealth;
    }

    // Handles item pickup
    void itemPickUp()
    {
        // If the player is pressing F 
        if(Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            // sends a raycast forwards from the camera 
            if(Physics.Raycast(playerCameraRotation.position, playerCameraRotation.forward, out hit, interactionDistance))
            {
                // if the raycast is a weapon
                if (hit.collider.tag == "Weapon")
                {
                    // Drop current weapon if one is already held
                    if (CurrentWeapon != null)
                    {
                        CurrentWeapon.DropWeapon();
                    }
                    // gets the weapon script from the weapon
                    CurrentWeapon = hit.collider.gameObject.GetComponent<weapon>();

                    // pick up weapon
                    CurrentWeapon.pickUpWeapon(playerCameraRotation);
                }
            }
        // if the player is pressing Q and is holding a weapon
        } else if (Input.GetKeyDown(KeyCode.Q) && CurrentWeapon != null)
        {
            // drop weapon
            CurrentWeapon.DropWeapon();
        }
    }

    // handles interacting
    void interact()
    {
        // casts a ray forward from the camera
        RaycastHit hit;
        if(Physics.Raycast(playerCameraRotation.position, playerCameraRotation.forward, out hit, interactionDistance))
        {
            // if the ray its a button
            if(hit.collider.tag == "Interactable Button")
            {
                // press the button
                interactableButton button = hit.collider.GetComponentInParent<interactableButton>();
                if (!button.pressed)
                {
                    button.press();
                }
            }
        }
    }

    // when called removes the ammount of input health from the player
    public void removeHealth(float damage)
    {
        playerHealth -= damage;
        healthBar.setHealth(playerHealth);

        // if health is less then or equal to 0 call playerDie()
        if (playerHealth <= 0) 
        {
            playerDie();
        }
    }

    // Called when the playerDies
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

    // called on the frame the player collides with a collider
    private void OnCollisionEnter(Collision collision)
    {
        // if the player collided with a dealth zone the player will die
        if (collision.gameObject.tag == "Death Zone")
        {
            playerDie();
        }
    }
}
