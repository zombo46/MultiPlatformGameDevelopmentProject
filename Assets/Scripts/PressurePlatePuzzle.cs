using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlatePuzzle : MonoBehaviour
{
    public PuzzleSystem door;
    public float closeDelay = 5f;
    private Coroutine close;
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (close != null) {
                StopCoroutine(close);
                close = null;
            }
            door.OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            close = StartCoroutine(CloseDoor());
        }
    }

    private IEnumerator CloseDoor() {
        yield return new WaitForSeconds(closeDelay);
        door.CloseDoor();
        close = null;
    }
}
