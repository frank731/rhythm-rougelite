using UnityEngine;
using UnityEngine.EventSystems;

public class ShowDescription : MonoBehaviour
{
    public GameObject descriptionHolder;
    public TMPro.TextMeshProUGUI descriptionText;
    public string description;

    private void Start()
    {
        descriptionText.text = description;
    }
    public void ShowItemDescription()
    {
        descriptionHolder.SetActive(!descriptionHolder.activeSelf);
        EventSystem.current.SetSelectedGameObject(descriptionHolder);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        //Mouse was clicked outside
        descriptionHolder.SetActive(false);
    }
}
