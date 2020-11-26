using UnityEngine;

public class ChestController : MonoBehaviour
{
    public Sprite closedChestSprite;
    public Sprite openChestSprite;
    public SpriteRenderer spriteRenderer;
    public PlayerInteractDetection playerInteractDetection;
    public GameObject[] itemPool;
    public bool opened = false;
    void Start()
    {
        playerInteractDetection.interact.AddListener(CreateItem);
    }
    void CreateItem()
    {
        if (!opened)
        {
            GameObject item = itemPool[Random.Range(0, itemPool.Length)];
            Instantiate(item, transform.position - new Vector3(0, 2, 0), transform.rotation);
            spriteRenderer.sprite = openChestSprite;
            opened = true;
        }
    }
}
