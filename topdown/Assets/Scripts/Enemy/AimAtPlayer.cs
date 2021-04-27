using UnityEngine;

public class AimAtPlayer : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public SpriteRenderer enemySprite;
    public SpriteRenderer gunSprite;
    public Transform enemy;
    private bool facingRight = true;
    [SerializeField]
    private EnemyController enemyController;
    void Start()
    {
        //prevent enemyController start function from being called
    }
    void Update()
    {
        if (enemyController.isActive)
        {
            //flips the gun and enemy left is player is left of enemy
            //transform.localScale = new Vector3(1f, -1f, 1f);
            if (enemyController.player.position.x < enemy.position.x && facingRight)
            {
                gunSprite.flipY = true;
                transform.position = leftHand.position;
                enemySprite.flipX = true;
                facingRight = false;
            }
            //flips the gun and enemy right is player is right of enemy
            if (enemyController.player.position.x > enemy.position.x && !facingRight)
            {
                gunSprite.flipY = false;
                transform.position = rightHand.position;
                enemySprite.flipX = false;
                facingRight = true;
            }
            //rotates gun towards player
            Vector2 direction = new Vector2(enemyController.player.position.x - transform.position.x, enemyController.player.position.y - transform.position.y);
            transform.right = direction;
        }

    }
}
