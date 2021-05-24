using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//[CreateAssetMenu(fileName = "New Ability", menuName = "Ability", order = 1)]
public class Ability : Item
{
    public int cooldown;
    public bool onCooldown = false;
    public GameObject pickup;
    public FloorGlobal floorGlobal;

    private GameObject image;
    private GameObject cooldownUI;
    private TMPro.TextMeshProUGUI cooldownText;
    private int currentCooldown;
    private long startBeat;

    public void SetUI(GameObject newUI)
    {
        image = newUI;
        image.GetComponent<Image>().sprite = itemSprite;
        cooldownUI = image.transform.GetChild(0).gameObject;
        cooldownText = cooldownUI.GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    public void StartCooldown()
    {
        Debug.Log("started");
        currentCooldown = cooldown;
        onCooldown = true;
        cooldownUI.SetActive(true);
        cooldownText.text = currentCooldown.ToString();
        startBeat = floorGlobal.beatNumber;
    }

    public bool UpdateCooldown()
    {
        if (onCooldown && floorGlobal.beatNumber != startBeat)
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
