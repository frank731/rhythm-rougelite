using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public PlayerData playerData;
    public void OnAbilityOne()
    {
        if(playerData.abilities[0] != null)
        {
            playerData.abilities[0].action();
        }
    }
    public void OnAbilityTwo()
    {
        if (playerData.abilities[1] != null)
        {
            playerData.abilities[1].action();
        }
    }
    public void OnAbilityThree()
    {
        if (playerData.abilities[2] != null)
        {
            playerData.abilities[2].action();
        }
    }
}
