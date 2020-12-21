using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Item", menuName = "Items", order = 1)]
public class Item : ScriptableObject
{
    public int itemId;
    public string displayName;
    public string description;
    public Sprite itemSprite;
    public UnityAction action;
    public GameObject inventoryIcon;
}
