using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public static float bulletSpeed = 10;
    public static float bulletDamage = 1;
    void FixedUpdate()
    {
        //moves the bullet
        rb.velocity = transform.right * bulletSpeed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //destroys bullet if it hits something, lowers health 
        
        if (collision.collider.tag == "Enemy")
        {
            collision.collider.gameObject.GetComponent<enemyController>().removeHealth(1);
        }
        if(collision.collider.tag == "Player")
        {
            collision.collider.gameObject.GetComponent<playerController>().removeHealth(1);
        }

        Destroy(gameObject);

    }
}
