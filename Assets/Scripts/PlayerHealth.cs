using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public Image healthBar;

    public GameObject screen;
    private DeathScreen deathScreen;

    float currentHealth;
    public float maxHealth = 10;
    public float damageAmount;
    public static bool isPlayerDead = false;
    public static bool isTakingDamage = false;
    float lerpSpeed;

    private void Start()
    {
        isPlayerDead = false;
        deathScreen = screen.GetComponent<DeathScreen>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        lerpSpeed = 3f * Time.deltaTime;

        HealthBarFill();
        ColorChange();
    }

    void HealthBarFill()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, (currentHealth / maxHealth), lerpSpeed);
    }

    void ColorChange()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (currentHealth / maxHealth));
        healthBar.color = healthColor;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyProjectile")
        {
            PlayerDamage();
        }
    }

    public void PlayerDamage()
    {
        if(currentHealth > 0)
        {
            if(damageAmount >= currentHealth)
            {
                isTakingDamage = true;
                PlayerDead();
            }
            else
            {
                isTakingDamage = true;
                currentHealth -= damageAmount;
            }
        }
    }

    void PlayerDead()
    {
        currentHealth = 0;
        isPlayerDead = true;
        deathScreen.GameOverMessage();
    }
}
