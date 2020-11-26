using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Guns", order = 0)]
public class Gun : ScriptableObject
{
    public int itemId;
    public string displayName;
    public string description;
    public Sprite gunSprite;
    public GameObject gunObject;
}
