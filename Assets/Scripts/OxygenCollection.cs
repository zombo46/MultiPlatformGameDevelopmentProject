using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OxygenCollection : MonoBehaviour
{
    public float newOxygen = 5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerVitality vitality = other.GetComponent<PlayerVitality>();
            if (vitality != null) {
                vitality.currentOxygen = vitality.currentOxygen + newOxygen;
                if (vitality.currentOxygen > vitality.maxOxygen) {
                    vitality.currentOxygen = vitality.maxOxygen;
                }
                vitality.SendMessage("updateUI", SendMessageOptions.DontRequireReceiver);
            }

            Destroy(gameObject);
        }
    }
}
