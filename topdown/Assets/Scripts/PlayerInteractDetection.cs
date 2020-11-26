using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractDetection : MonoBehaviour
{
    public bool inRange = false;
    public bool interacted = false;
    public UnityEvent interact = new UnityEvent();
    private void Start()
    {
        FloorGlobal.Instance.pausableScripts.Add(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false;
        }
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
