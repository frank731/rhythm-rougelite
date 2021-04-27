using UnityEngine;
using UnityEngine.InputSystem;

public class AimAtCursor : MonoBehaviour
{
    public Transform player;
    public bool mouseFacingRight = true;
    public Transform leftHand = null;
    public Transform rightHand = null;
    public SpriteRenderer playerSprite;
    private Vector2 direction;
    private Vector3 mousePos;
    private void Start()
    {
        FloorGlobal.Instance.pausableScripts.Add(this);
        player = transform.parent.parent;
        playerSprite = player.GetComponent<SpriteRenderer>();
        foreach (Transform child in player)
        {
            if (child.CompareTag("LeftHand"))
            {
                leftHand = child;
                if (rightHand)
                {
                    break;
                }
            }
            else if (child.CompareTag("RightHand"))
            {
                rightHand = child;
                if (leftHand)
                {
                    break;
                }
            }
        }
        transform.localScale = new Vector3(1, 1, 1);
        transform.SetParent(rightHand);
        transform.position = rightHand.position;
        playerSprite.flipX = false;
        mouseFacingRight = true;
    }
    void FixedUpdate()
    {
        mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        //flip gun and player left when mouse moves to the left of player
        if (mousePos.x < player.position.x && mouseFacingRight == true)
        {
            transform.localScale = new Vector3(1, -1, 1); //cant use vector.set for some reason idk
            transform.SetParent(leftHand);
            transform.position = leftHand.position;
            playerSprite.flipX = true;
            mouseFacingRight = false;
        }
        //flip gun and player right when mouse moves to the right of player
        if (mousePos.x > player.position.x && mouseFacingRight == false)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.SetParent(rightHand);
            transform.position = rightHand.position;
            playerSprite.flipX = false;
            mouseFacingRight = true;
        }
        //rotate gun towards mouse
        direction.Set(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

        transform.right = direction;
    }

}
