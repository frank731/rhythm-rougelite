using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aimAtPlayer : enemyController
{
    public Transform leftHand;
    public Transform rightHand;
    public SpriteRenderer enemySprite;
    public SpriteRenderer gunSprite;
    public Transform enemy;
    private Transform player;
    private bool facingRight = true;
    private void Start()
    {
        player = getPlayer();
    }
    void Update()
    {
        if (isActive == true)
        {
            //flips the gun and enemy left is player is left of enemy
            //transform.localScale = new Vector3(1f, -1f, 1f);
            if (player.position.x < enemy.position.x && facingRight == true)
            {
                gunSprite.flipY = true;
                transform.position = leftHand.position;
                enemySprite.flipX = true;
                facingRight = false;
            }
            //flips the gun and enemy right is player is right of enemy
            if (player.position.x > enemy.position.x && facingRight == false)
            {
                gunSprite.flipY = false;
                transform.position = rightHand.position;
                enemySprite.flipX = false;
                facingRight = true;
            }
            //rotates gun towards player
            Vector2 direction = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y);
            transform.right = direction;
        }
        
    }
}
