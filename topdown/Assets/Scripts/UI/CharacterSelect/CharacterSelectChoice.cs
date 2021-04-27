using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Select", menuName = "Character Select", order = 1)]
public class CharacterSelectChoice : ScriptableObject
{
    public Sprite displaySprite;
    public Sprite profileSprite;
    public string charName;
    public string description;
    public int charID;   
}
