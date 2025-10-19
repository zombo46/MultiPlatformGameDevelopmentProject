using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OxygenSystem : MonoBehaviour
{
    public float maxOxygen = 100f;
    public float currentOxygen;
    public Slider oxygenBar;
    public Text oxygenText;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        currentOxygen = maxOxygen;
        updateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) {
            return;
        }
        currentOxygen = currentOxygen - Time.deltaTime;
        if (currentOxygen <= 0) {
            currentOxygen = 0;
            PlayerDeath();
        }
        updateUI();
    }

    void updateUI() {
        if (oxygenBar != null) {
            oxygenBar.value = currentOxygen / maxOxygen;
        }
        if (oxygenText != null) {
            oxygenText.text = $"{Mathf.Ceil(currentOxygen)}s";
        }
    }

    void PlayerDeath() {
        isDead = true;

    }
}
