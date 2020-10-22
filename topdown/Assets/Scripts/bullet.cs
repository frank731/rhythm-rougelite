using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
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
        Destroy(gameObject);

        if (collision.collider.tag == "Enemy")
        {
            collision.collider.gameObject.GetComponent<EnemyController>().RemoveHealth(bulletDamage);
        }
        if(collision.collider.tag == "Player")
        {
            collision.collider.gameObject.GetComponent<PlayerController>().RemoveHealth(bulletDamage);
        }
        

    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
