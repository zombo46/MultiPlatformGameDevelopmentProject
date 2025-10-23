using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerVitality : MonoBehaviour
{
    public float maxOxygen = 100f;
    public float currentOxygen;
    public Slider oxygenBar;
    public Text oxygenText;
    private bool isDead = false;

    public float PlayerMaxHealth = 100f;

    public float PlayerCurrentHealth;

    public Slider HealthBar;

    public Text HealthText;

    // Start is called before the first frame update
    void Start()
    {
        currentOxygen = maxOxygen;
        PlayerCurrentHealth = PlayerMaxHealth;
        updateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }
        currentOxygen = currentOxygen - Time.deltaTime;
        if (currentOxygen <= 0)
        {
            currentOxygen = 0;
            PlayerDeath();
        }

        if (PlayerCurrentHealth <= 0)
        {
            PlayerCurrentHealth = 0;
            PlayerDeath();
        }
        updateUI();
    }

    public void ReduceHealth(float Amount)
    {
        PlayerCurrentHealth -= Amount;
    }

    void updateUI() {
        if (oxygenBar != null) {
            oxygenBar.value = currentOxygen / maxOxygen;
        }
        if (oxygenText != null)
        {
            oxygenText.text = $"Oxygen level: {Mathf.Ceil(currentOxygen)}%";
        }

        if (HealthBar != null)
        {
            HealthBar.value = PlayerCurrentHealth / PlayerMaxHealth;
        }

        if (HealthText != null)
        {
            HealthText.text = $"HP: {Mathf.Ceil(PlayerCurrentHealth)}";
        }
    }

    void PlayerDeath() {
        isDead = true;
        Debug.Log("Game Over!");
    }
}
