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
    public GameObject pickup;

    private GameObject image;
    private GameObject cooldownUI;
    private TMPro.TextMeshProUGUI cooldownText;
    private int currentCooldown;
    private long startBeat;

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
        startBeat = FloorGlobal.Instance.beatNumber;
    }

    public bool UpdateCooldown()
    {
        if (onCooldown && FloorGlobal.Instance.beatNumber != startBeat)
        {
            currentCooldown -= 1;
            if (currentCooldown == 0)
            {
                onCooldown = false;
                cooldownUI.SetActive(false);
                return true;
            }
        }
        cooldownText.text = currentCooldown.ToString();
        return false;
    }
    
}
