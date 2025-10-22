using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSystem : MonoBehaviour
{
    public float height = 1f;
    public float speed = 1f;
    private Vector3 closed;
    private Vector3 open;
    private bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        closed = transform.position;
        open = closed + new Vector3(0, height, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target;
        if (isOpen) {
            target = open;
        } else {
            target = closed;
        }
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
    }

    public void OpenDoor(){
        isOpen = true;
    }

    public void CloseDoor() {
        isOpen = false;
    }
}
