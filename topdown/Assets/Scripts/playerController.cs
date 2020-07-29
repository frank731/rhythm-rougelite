using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public int health = 3;
    public bool isDead = false;
    // Update is called once per frame
    public void removeHealth(int damage)
    {
        health -= damage;
        //destroys player once health is lower than or equal to zero
        if (health <= 0)
        {
            isDead = true;
            Debug.Log("dead");
            //Destroy(gameObject);
        }
    }
}
