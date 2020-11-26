using UnityEngine;

public class AddItem : MonoBehaviour
{
    public int itemId;
    public string itemType;
    private PlayerController PlayerController;
    public PlayerInteractDetection playerInteractDetection;
    public void Awake()
    {
        PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    public void Start()
    {
        playerInteractDetection.interact.AddListener(OnInteract);
    }
    public void OnInteract()
    {
        PlayerController.AddItem(itemId, itemType);
        Destroy(gameObject);
    }
}
