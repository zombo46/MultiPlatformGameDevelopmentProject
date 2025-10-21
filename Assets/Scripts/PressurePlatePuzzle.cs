using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlatePuzzle : MonoBehaviour
{
    public Puzzle door;
    
    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player")) {
            door.OpenDoor();
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.CompareTag("Player")) {
            door.CloseDoor();
        }
    }
}
