using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowDescription : MonoBehaviour
{
    public GameObject descriptionHolder;
    public TMPro.TextMeshPro descriptionText;
    public string description;

    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(descriptionHolder);
        descriptionText.text = description;
    }
    void ShowItemDescription()
    {
        descriptionHolder.SetActive(true);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        //Mouse was clicked outside
        descriptionHolder.SetActive(false);
    }
}
