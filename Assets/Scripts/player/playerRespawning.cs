using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerRespawning : MonoBehaviour
{
    public GameObject player; // the players gameobject.
    public GameObject healthBar; // Players health bar.
    public GameObject playerCam; // The Players camera.

    public Vector3 spawnPoint; // Spawn Point of the player.
    public Quaternion spawnRotation; // Spawn Rotation of the players.

    public bool alive; // bool that displays wether the player is in an alive of dead state.
    private playerCombat playerCombatScript; // players playerCombatScript.

    // Called on the first frame.
    private void Start()
    {
        // Gets spawnpoint and spawn rotation.
        spawnPoint = player.transform.position;
        spawnRotation = Quaternion.Euler(0, 0, 0);

        // Gets the players combat script.
        playerCombatScript = player.GetComponent<playerCombat>();

        // Initalise alive variable.
        alive = true;
    }

    // Called on every frame.
    private void Update()
    {
        // Respawns the player when they click any key when they are dead
        if (!alive && Input.anyKeyDown)
        {
            respawn();
        }
    }

    // Respawns the player
    public void respawn()
    {
        alive = true; // Set alive to true

        // Activate healthbar and player
        healthBar.SetActive(true);
        player.SetActive(true);

        // Moves player to spawn point
        player.transform.position = spawnPoint;

        // Heals player to full health
        playerCombatScript.maxHeal();

        // Sets players velocity to 0 when respawning
        Rigidbody playerRb = player.GetComponent<Rigidbody>();

        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        // Moves player camera to the spawn roatation
        playerCam.transform.rotation = spawnRotation;
    }

}
