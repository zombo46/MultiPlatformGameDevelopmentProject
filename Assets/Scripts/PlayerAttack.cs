using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IInteractable
{
    public void Interact(Collider collider)
    {
        Debug.Log("Hello");
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Interact!");
            Destroy(collider.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
