using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenCollection : MonoBehaviour
{
    void onTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
    }
}
