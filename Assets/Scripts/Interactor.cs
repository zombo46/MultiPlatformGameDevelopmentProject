using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

interface IInteractable
{
   void Interact(Collider collider);
}
public class Interactor : MonoBehaviour
{
    public Transform interactionPoint;
    public float interactionRange = 2f;

    void Start()
    {
        if (interactionPoint == null)
        {
            interactionPoint = this.transform;
        }
    }

    void Update()
    {
        // Input System only (keyboard-only)
        bool interactPressed = (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame);

        if (interactPressed)
            DoInteraction();
    }

    private void DoInteraction()
    {
        Collider[] colliders = Physics.OverlapSphere(interactionPoint.position, interactionRange);
        foreach (Collider collider in colliders)
        {
            IInteractable interactable = collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(collider);
                break;
            }
        }
    }
}