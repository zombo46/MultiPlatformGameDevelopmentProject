using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script allows the player to collect a repair component (artifact) when they collide with it.

public class RepairComponentCollection : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
            playerInventory.AddItem("Artifact");
            Destroy(gameObject);
        }
    }
}