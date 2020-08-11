using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractDetection : MonoBehaviour
{
    public bool inRange = false;
    public bool interacted = false;
    public UnityEvent interact = new UnityEvent();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        inRange = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        inRange = false;
    }
    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            interacted = true;
            interact.Invoke();
        }
    }
}
