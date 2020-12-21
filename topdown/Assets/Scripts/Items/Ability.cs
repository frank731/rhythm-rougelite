using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability", order = 1)]
public class Ability : ScriptableObject
{
    public int abilityID;
    public string displayName;
    public string description;
    public Sprite abilitySprite;
    public UnityAction action;
}
