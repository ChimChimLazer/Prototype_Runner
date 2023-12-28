using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerRespawning : MonoBehaviour
{
    public GameObject player;
    public Vector3 spawnPoint;

    private bool alive;

    private void Start()
    {
        spawnPoint = player.transform.position;
        alive = true;
    }

    private void Update()
    {
        if (!alive && Input.GetKeyDown(KeyCode.Space))
        {
            respawn();
        }
    }

    public void die()
    {
        alive = false;
        print(alive);
    }

    public void respawn()
    {
        alive = true;
        player.SetActive(true);
        player.transform.position = spawnPoint;
    }

}
