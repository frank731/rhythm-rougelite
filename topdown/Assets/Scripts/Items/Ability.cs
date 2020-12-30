using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability", order = 1)]
public class Ability : ScriptableObject
{
    public int abilityID;
    public int cooldown;
    public string displayName;
    public string description;
    public Sprite abilitySprite;
    public UnityAction action;
    public bool onCooldown = false;

    private GameObject image;
    private GameObject cooldownUI;
    private TMPro.TextMeshProUGUI cooldownText;
    private int currentCooldown;

    public void SetUI(GameObject newUI)
    {
        image = newUI;
        image.GetComponent<Image>().sprite = abilitySprite;
        cooldownUI = image.transform.GetChild(0).gameObject;
        cooldownText = cooldownUI.GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    public void StartCooldown()
    {
        currentCooldown = cooldown;
        onCooldown = true;
        cooldownUI.SetActive(true);
        cooldownText.text = currentCooldown.ToString();
    }

    public bool UpdateCooldown()
    {
        Debug.Log(currentCooldown);
        cooldownText.text = currentCooldown.ToString();
        if (onCooldown)
        {
            currentCooldown -= 1;
            if (currentCooldown == 0)
            {
                onCooldown = false;
                cooldownUI.SetActive(false);
                return true;
            }
        }
        return false;
    }
    
}
