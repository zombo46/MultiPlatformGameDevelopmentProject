using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVitality : MonoBehaviour
{
    public float maxOxygen = 100;
    public float currentOxygen;
    public Slider oxygenBar;
    public Text oxygenText;
    private bool isDead = false;

    public float PlayerMaxHealth = 100;
    public float PlayerCurrentHealth;
    public Slider HealthBar;
    public Text HealthText;

    // When true, oxygen does not tick down.
    private bool oxygenPaused = true;

    void Start()
    {
        currentOxygen = maxOxygen;
        PlayerCurrentHealth = PlayerMaxHealth;
        updateUI();
    }

    void Update()
    {
        if (isDead)
            return;

        if (!oxygenPaused)
        {
            currentOxygen -= Time.deltaTime;
            if (currentOxygen <= 0)
            {
                currentOxygen = 0;
                PlayerDeath();
            }
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

    public void revive()
    {
        isDead = false;
        currentOxygen = maxOxygen;
        PlayerCurrentHealth = PlayerMaxHealth;

        this.transform.position = new Vector3(135f, 1f, 132f);
    }

    void updateUI()
    {
        if (oxygenBar != null)
        {
            if (oxygenBar.maxValue > 1f)
                oxygenBar.value = Mathf.Clamp(currentOxygen, oxygenBar.minValue, oxygenBar.maxValue);
            else
                oxygenBar.value = Mathf.Clamp01(currentOxygen / maxOxygen);
        }

        if (oxygenText != null)
            oxygenText.text = $"Oxygen level: {Mathf.Ceil(currentOxygen)}%";

        if (HealthBar != null)
        {
            if (HealthBar.maxValue > 1f)
                HealthBar.value = Mathf.Clamp(PlayerCurrentHealth, HealthBar.minValue, HealthBar.maxValue);
            else
                HealthBar.value = Mathf.Clamp01(PlayerCurrentHealth / PlayerMaxHealth);
        }

        if (HealthText != null)
            HealthText.text = $"HP: {Mathf.Ceil(PlayerCurrentHealth)}";
    }

    void PlayerDeath()
    {
        isDead = true;
        this.SendMessage("ShowDeath");
    }

    // Public to pause or unpause oxygen depletion in other scripts.
    public void SetOxygenPaused(bool paused)
    {
        oxygenPaused = paused;
    }

    public bool IsOxygenPaused()
    {
        return oxygenPaused;
    }
}
