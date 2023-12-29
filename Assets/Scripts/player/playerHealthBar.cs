using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealthBar : MonoBehaviour
{
    public playerRespawning player;
    private Slider healthBar;
    private Image bar;
    Color barColor;

    void Start()
    {
        healthBar = GetComponent<Slider>();
        healthBar.value = healthBar.maxValue;

        bar = healthBar.fillRect.GetComponent<Image>();
        barColor = bar.color;
    }

    private void Update()
    {
        if (!player.alive)
        {
            gameObject.SetActive(false);
        }

        healthBarTransperency();
    }

    public void setHealth(float health)
    {
        healthBar.value = health;
    }

    private void healthBarTransperency()
    {
        float targetTrans;
        float newTrans;
        if (healthBar.value < 100)
        {
            targetTrans = 0.9f;
            newTrans = Mathf.Lerp(barColor.a, targetTrans, Time.deltaTime * 20);
        }
        else
        {
            targetTrans = 0;
            newTrans = Mathf.Lerp(barColor.a, targetTrans, Time.deltaTime * 5);
        }
        
        barColor.a = newTrans;
        bar.color = barColor;
    }
}
