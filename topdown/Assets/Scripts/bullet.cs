using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject bulletDestroyedEffect;
    public float bulletSpeed = 10;
    public float bulletDamage = 1;
    void FixedUpdate()
    {
        //moves the bullet
        rb.velocity = transform.right * bulletSpeed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //destroys bullet if it hits something, lowers health 

        Instantiate(bulletDestroyedEffect, transform.position, transform.rotation);
        if (collision.collider.CompareTag("Enemy"))
        {
            collision.collider.gameObject.GetComponent<EnemyController>().RemoveHealth(bulletDamage);
        }
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.gameObject.GetComponent<PlayerController>().RemoveHealth(bulletDamage);
        }

        Destroy(gameObject);
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
