using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerRespawning : MonoBehaviour
{
    public GameObject player;
    public GameObject healthBar;

    public Vector3 spawnPoint;

    public bool alive;
    private playerCombat playerCombatScript;

    private void Start()
    {
        spawnPoint = player.transform.position;
        playerCombatScript = player.GetComponent<playerCombat>();
        alive = true;
    }

    private void Update()
    {
        if (!alive && Input.GetKeyDown(KeyCode.Space))
        {
            respawn();
        }
    }

    public void respawn()
    {
        alive = true;
        healthBar.SetActive(true);
        player.SetActive(true);
        player.transform.position = spawnPoint;
        playerCombatScript.maxHeal();

        // https://forum.unity.com/threads/remove-all-force-innertia-from-a-rigidbody.25026/
        // http://unity3d.com/support/documentation/ScriptReference/Rigidbody-velocity.html

        // Sets players velocity to 0 when respawning
        Rigidbody playerRb = player.GetComponent<Rigidbody>();

        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
    }

}
