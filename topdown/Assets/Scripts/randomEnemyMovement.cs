using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class randomEnemyMovement : enemyController
{
    public Transform enemyGun;

    public float speed = 1f;

    public bool canMove = true;
    public float moveDelay;

    public float moveDistance;
    public Vector3 moveDirection;

    public Rigidbody2D rb;
    private void Awake()
    {
        //initalize values for random values
        moveDelay = Random.Range(2f, 4f);
        moveDistance = Random.Range(1f, 3f);
        moveDirection = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * Vector3.up;
    }
    void FixedUpdate()
    {
        //move the enemy in a random direction and distance at random intervals
        if(canMove == true && isActive)
        {
            StartCoroutine(waitOnSpot());
            rb.velocity = moveDirection * speed;
        }

    }
    public IEnumerator waitOnSpot()
    {
        canMove = false;
        yield return new WaitForSeconds(moveDelay); 
        //set random distances and times
        moveDelay = Random.Range(2f, 4f);
        moveDistance = Random.Range(1f, 3f);
        moveDirection = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * Vector3.up;
        canMove = true;
    }
}
