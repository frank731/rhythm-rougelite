using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Choices", menuName = "Character Choices", order = 1)]

public class CharacterChoicesObject : ScriptableObject
{
    public List<GameObject> characters;
}
