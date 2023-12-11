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

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = maxHealth;
        lastFrameHealth = playerHealth;
        regenerationCoolDown = regenerationCoolDownTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            playerHealth -= 10;
        }
        healthRegeneration();
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
}
