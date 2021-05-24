using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseAbilitySlot : MonoBehaviour
{
    public Button[] slots;
    public PlayerController playerController;
    private Ability newAbility; 
    public void ChooseSlot(int slot)
    {
        Time.timeScale = 1;
        if (playerController.abilities[slot] != null)
        {
            Instantiate(playerController.abilities[slot].pickup, playerController.transform.position, playerController.transform.rotation);
        }
        playerController.AddAbility(slot, newAbility);
        slots[slot].image.sprite = newAbility.itemSprite;
        gameObject.SetActive(false);
    }

    public void SetNewAbility(Ability ability)
    {
        newAbility = ability;
    }
}
