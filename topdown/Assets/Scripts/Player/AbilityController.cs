using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityController : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject[] abilityCooldownUI;
    private List<Ability> usedAbilities = new List<Ability>();
    private FloorGlobal floorGlobal;

    private void Start()
    {
        floorGlobal = FloorGlobal.Instance;
        floorGlobal.onBeat.AddListener(UpdateCooldowns);
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
    public void OnAbility(int index)
    {
        if (playerController.abilities[index] != null)
        {
            Debug.Log(playerController.abilities[index].onCooldown);
            if (!playerController.abilities[index].onCooldown && floorGlobal.IsOnBeat() != 0){
                
                playerController.abilities[index].itemEffect.OnUse();
                playerController.abilities[index].StartCooldown();
                usedAbilities.Add(playerController.abilities[index]);
            }
        }
    }
    public void OnAbilityOne()
    {
        OnAbility(0);
    }
    public void OnAbilityTwo()
    {
        OnAbility(1);
    }
    public void OnAbilityThree()
    {
        OnAbility(2);
    }
}
