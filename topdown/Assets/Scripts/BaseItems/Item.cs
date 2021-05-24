using UnityEngine;
using UnityEngine.Events;

//[CreateAssetMenu(fileName = "New Item", menuName = "Items", order = 1)]
public class Item : MonoBehaviour
{
    public int itemId;
    public string type;
    public string displayName;
    public string description;
    public Sprite itemSprite;
    public bool useUpdate;
    public bool usePickup;
    public bool useHit;
    public PlayerController playerController;
    public PlayerMovement playerMovement;
    public ItemEffect itemEffect;
    public void Awake()
    {
        itemEffect.item = this;
    }
}
