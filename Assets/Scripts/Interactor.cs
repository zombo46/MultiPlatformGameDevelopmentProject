using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

interface IInteractable
{
   // void Interact(Collider collider);
}
public class Interactor : MonoBehaviour
{
    public Transform interactionPoint;
    public float interactionRange = 2f;
    // Start is called before the first frame update
    void Start()
    {
       // if (interactionPoint == null)
        {
            //interactionPoint = this.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        {
            //Collider[] colliders = Physics.OverlapSphere(interactionPoint.position, interactionRange);
            //foreach (Collider collider in colliders)
            {
              //  IInteractable interactable = collider.GetComponent<IInteractable>();
                //if (interactable != null)
                {
                  //  interactable.Interact(collider);
                   // break;
                }
            }
        }
    }
}