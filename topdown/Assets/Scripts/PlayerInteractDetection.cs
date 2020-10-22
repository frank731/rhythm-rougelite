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
    private FloorGlobal floorGlobal;
    private void Start()
    {
        floorGlobal = GameObject.FindGameObjectWithTag("FloorGlobalHolder").GetComponent<FloorGlobal>();
        floorGlobal.pausableScripts.Add(this);
    }
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
        if (inRange && Input.GetKeyDown(KeyCode.F))
        {
            interacted = true;
            interact.Invoke();
        }
    }
}
