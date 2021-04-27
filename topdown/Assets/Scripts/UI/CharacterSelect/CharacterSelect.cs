using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

public class CharacterSelect : MonoBehaviour
{
    public CharacterSelectChoice[] choices;
    public int index = 0;
    private int choicesSize;
    private float holderXPos;
    private float frameXPos;
    private readonly int shiftSize = 160;
    [SerializeField]
    private Transform choicesHolder;
    [SerializeField]
    private Transform frame;
    [SerializeField]
    private TextMeshProUGUI desc;
    [SerializeField]
    private TextMeshProUGUI cname;
    [SerializeField]
    private Image profileImage;
    public void Start()
    {
        choicesSize = choices.Length - 1;
        holderXPos = choicesHolder.localPosition.x; //made so tween calc not using position inbetween tween
        frameXPos = frame.localPosition.x;
    }

    private void UpdateInfo()
    {
        CharacterSelectChoice choice = choices[index];
        profileImage.sprite = choice.profileSprite;
        cname.text = choice.charName;
        desc.text = choice.description;
    }
    public void OnNavigate(InputValue input)
    {
        float direction = input.Get<Vector2>().x;
        if (direction < 0 && index > 0)
        {
            holderXPos += shiftSize;
            frameXPos -= shiftSize;
            frame.DOLocalMoveX(frameXPos, 1f);
            //choicesHolder
                //.DOLocalMoveX(holderXPos, 1f);
            index--;
        }
        else if(direction > 0 && index < choicesSize)
        {
            holderXPos -= shiftSize;
            frameXPos += shiftSize;
            frame.DOLocalMoveX(frameXPos, 1f);
            //choicesHolder
                //.DOLocalMoveX(holderXPos, 1f);
            index++;
        }
        //UpdateInfo();
    }
}
