using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealthBar : MonoBehaviour
{
    public playerRespawning player;
    private Slider healthBar;
    
    void Start()
    {
        healthBar = GetComponent<Slider>();
        healthBar.value = healthBar.maxValue;
    }

    private void Update()
    {
        if (!player.alive)
        {
            gameObject.SetActive(false);
        }
    }

    public void setHealth(float health)
    {
        healthBar.value = health;
    }
}
