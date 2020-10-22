using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun
{
    public int itemId;
    public string displayName;
    public string description;
    public Sprite gunSprite;
    public GameObject gunObject;
    public Gun(int itemId, string displayName, string description, Sprite sprite, GameObject gunObject)
    {
        this.itemId = itemId;
        this.displayName = displayName;
        this.description = description;
        this.gunSprite = sprite;
        this.gunObject = gunObject;
    }
}
