using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    float speed = 3f;
    public Rigidbody2D rb;
    public Animator animator;
    void FixedUpdate()
    {
        //move the character
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        rb.velocity = (move * speed * Time.deltaTime * 50);
        // reverse character
        
        // change idle to run anim
        animator.SetFloat("player_speed", Mathf.Max(Mathf.Abs(rb.velocity.y), Mathf.Abs(rb.velocity.x)));
    }
}
