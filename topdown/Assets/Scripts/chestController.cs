using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public Sprite closedChestSprite;
    public Sprite openChestSprite;
    public SpriteRenderer spriteRenderer;
    public PlayerInteractDetection playerInteractDetection;
    public GameObject[] itemPool;
    void Start()
    {
        playerInteractDetection.interact.AddListener(CreateItem);
    }
    void CreateItem()
    {
        GameObject item = itemPool[Random.Range(0, itemPool.Length)];
        Instantiate(item, transform.position - new Vector3(0, 2, 0), transform.rotation);
        spriteRenderer.sprite = openChestSprite;
    }
}
