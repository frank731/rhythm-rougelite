using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterChoice : MonoBehaviour
{
    public CharacterSelectChoice choice;
    [SerializeField]
    private CharacterDescPanel characterDescPanel;
    [SerializeField]
    private TransitionButton tb;
    public void SelectChar()
    {
        ES3.Save("charChoice", choice.charID);
        tb.LoadLevel(1);
    }
    public void PreviewChar()
    {
        characterDescPanel.charName.text = choice.name;
        characterDescPanel.desc.text = choice.description;
        characterDescPanel.profile.sprite = choice.displaySprite;
        characterDescPanel.characterChoice = this;
        characterDescPanel.Show();
    }
}
