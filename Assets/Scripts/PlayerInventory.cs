using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script manages the player's inventory, allowing items to be added and checked.

public class PlayerInventory : MonoBehaviour
{
    private HashSet<string> items = new HashSet<string>();

    public void AddItem(string itemID)
    {
        items.Add(itemID);
    }

    public bool HasItem(string itemID)
    {
        return items.Contains(itemID);
    }
}