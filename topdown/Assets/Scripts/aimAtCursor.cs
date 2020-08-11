using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class aimAtCursor : MonoBehaviour
{
    public Transform player;
    public bool mouseFacingRight = true;
    public Transform leftHand;
    public Transform rightHand;
    public SpriteRenderer playerSprite;
    Vector3 mousePos;
    void Update()
    {
        mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        
        //flip gun and player left when mouse moves to the left of player
        if(mousePos.x < player.position.x && mouseFacingRight == true)
        {
            transform.localScale = new Vector3(1f, -1f, 1f);
            transform.position = leftHand.position;
            playerSprite.flipX = true;
            mouseFacingRight = false;
        }
        //flip gun and player right when mouse moves to the right of player
        if (mousePos.x > player.position.x && mouseFacingRight == false)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.position = rightHand.position;
            playerSprite.flipX = false;
            mouseFacingRight = true;
        }
        //rotate gun towards mouse
        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

        transform.right = direction;


    }
    
}
