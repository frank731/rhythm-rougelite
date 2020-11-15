using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item 
{
    public int itemId;
    public string displayName;
    public string description;
    public Sprite itemSprite;
    public UnityAction function;
    public GameObject inventoryIcon;
    public Item(int itemId, string displayName, string description, Sprite sprite, UnityAction function)
    {
        this.itemId = itemId;
        this.displayName = displayName;
        this.description = description;
        this.itemSprite = sprite;
        this.function = function;
    }
}
