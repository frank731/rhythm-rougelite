using UnityEngine;
using UnityEngine.Events;

public class PlayerInteractDetection : MonoBehaviour
{
    public bool inRange = false;
    public bool interacted = false;
    public UnityEvent interact = new UnityEvent();
    public PlayerController playerController;
    public Material baseMat;
    public Material outlineMat;
    public Renderer spriteRenderer;
    private void Start()
    {
        FloorGlobal.Instance.pausableScripts.Add(this);
        if (baseMat == null) baseMat = Resources.Load<Material>("Materials/Lit");
        if (outlineMat == null) outlineMat = Resources.Load<Material>("Materials/Outline");
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>(); 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = true;
            playerController = collision.GetComponent<PlayerController>();
            spriteRenderer.material = outlineMat;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false;
            spriteRenderer.material = baseMat;
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
