using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class for the players healthbar
public class playerHealthBar : MonoBehaviour
{
    public playerRespawning player; // Players repsawning script

    public Image backGround; // Background behind the health bar
    private Slider healthBar; // Players health bar slider
    private Image bar; // the image on the health bar slider

    Color barColor; // colour of the health bar
    Color backGroundColor; // colour of the background

    // Called on the first frame
    void Start()
    {
        // gets the healthbar component
        healthBar = GetComponent<Slider>();
        // Sets the health bar to a max value
        healthBar.value = healthBar.maxValue;

        // Gets the health bar image
        bar = healthBar.fillRect.GetComponent<Image>();

        // Gets the colour of the health bar and background
        barColor = bar.color;
        backGroundColor = backGround.color;
    }

    // called every frame
    private void Update()
    {
        // if the player is dead, hides the healthbar
        if (!player.alive)
        {
            gameObject.SetActive(false);
        }

        // Handles health bar transperancy
        healthBarTransperency();
    }

    // Sets health to the value input when called
    // Called in the player combat script
    public void setHealth(float health)
    {
        healthBar.value = health;
    }

    // Handles the transperency of the health bar and background
    // This functions allows a smooth transition between showing and hiding the health bar
    private void healthBarTransperency()
    {
        float targetTrans; // target transperency
        float newTrans; // new transperency
        // if health bar is less then 100
        if (healthBar.value < 100)
        {
            // sets target tranparency
            targetTrans = 0.9f;
            newTrans = Mathf.Lerp(barColor.a, targetTrans, Time.deltaTime * 20); // gets the new transparency of for the current frame
        }
        else
        {
            // sets target tranparency
            targetTrans = 0;
            newTrans = Mathf.Lerp(barColor.a, targetTrans, Time.deltaTime * 5); // gets the new transparency of for the current frame
        }
        

        // gets the new colours for the background and bar
        barColor.a = newTrans;
        backGroundColor.a = newTrans;

        // sets the new colours for the background and bar
        bar.color = barColor;
        backGround.color = backGroundColor;
    }
}
