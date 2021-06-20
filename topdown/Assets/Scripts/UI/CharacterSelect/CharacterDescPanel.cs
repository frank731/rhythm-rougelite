using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CharacterDescPanel : MonoBehaviour
{
    public TextMeshProUGUI charName, desc;
    public Image profile;
    public CharacterChoice characterChoice;
    public float hidePos = -281, visPos = 0;
    private RectTransform rt;
    public MenuButtonController menuButtonController;
    private bool hidden = true;
    private void Start()
    {
        rt = GetComponent<RectTransform>();
        Hide();
    }
    public void Show()
    {
        if (hidden)
        {
            rt.anchoredPosition.Set(rt.anchoredPosition.x, hidePos);
            rt
                .DOAnchorPosY(visPos, 1f);
            hidden = false;
        }
    }
    public void Hide()
    {
        if (!hidden)
        {
            
            rt
                .DOAnchorPosY(hidePos, 1f);
            hidden = true;
        }
       
    }
    public void OnCancel()
    {
        if (!hidden)
        {
            Hide();
        }
        else
        {
            menuButtonController.OpenTitle();
        }
    }
    public void Play()
    {
        characterChoice.SelectChar();
    }
}
