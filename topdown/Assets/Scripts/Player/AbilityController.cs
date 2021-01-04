using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public PlayerData playerData;
    public GameObject[] abilityCooldownUI;
    private List<Ability> usedAbilities = new List<Ability>();

    private void Start()
    {
        FloorGlobal.Instance.onBeat.AddListener(UpdateCooldowns);
    }

    private void UpdateCooldowns()
    {
        
        for (int i = usedAbilities.Count - 1; i >= 0; i--)
        {
            
            if (usedAbilities[i].UpdateCooldown())
            {
                usedAbilities.RemoveAt(i);
            }
            
        }
    }

    public void OnAbilityOne()
    {
        if(playerData.abilities[0] != null && !playerData.abilities[0].onCooldown && FloorGlobal.Instance.isOnBeat)
        {
            playerData.abilities[0].action();
            playerData.abilities[0].StartCooldown();
            usedAbilities.Add(playerData.abilities[0]);
        }
    }
    public void OnAbilityTwo()
    {
        if (playerData.abilities[1] != null && !playerData.abilities[1].onCooldown && FloorGlobal.Instance.isOnBeat)
        {
            playerData.abilities[1].action();
            playerData.abilities[1].StartCooldown();
            usedAbilities.Add(playerData.abilities[1]);
        }
    }
    public void OnAbilityThree()
    {
        if (playerData.abilities[2] != null && !playerData.abilities[2].onCooldown && FloorGlobal.Instance.isOnBeat)
        {
            playerData.abilities[2].action();
            playerData.abilities[2].StartCooldown();
            usedAbilities.Add(playerData.abilities[2]);
        }
    }
}
