using UnityEngine;

public class AddItem : MonoBehaviour
{
    public int itemId;
    public string itemType;
    public PlayerInteractDetection playerInteractDetection;
    public void Start()
    {
        playerInteractDetection.interact.AddListener(OnInteract);
    }
    public void OnInteract()
    {
        playerInteractDetection.playerController.AddItem(itemId, itemType);
        Destroy(gameObject);
    }
}
