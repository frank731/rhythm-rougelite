using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chestController : MonoBehaviour
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
        Debug.Log("ok");
    }
}
