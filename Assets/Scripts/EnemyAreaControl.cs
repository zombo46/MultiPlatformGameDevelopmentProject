using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAreaControl : MonoBehaviour
{

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AlertEnemy(other.gameObject);
        }
    }

    void AlertEnemy(GameObject player)
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            this.gameObject.transform.GetChild(i).SendMessage("SetPath", player);
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
