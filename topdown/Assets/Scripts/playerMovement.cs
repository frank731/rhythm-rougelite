using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    public Rigidbody2D rb;
    public Animator animator;
    Vector2 move;
    Vector2 direction;
    public void FixedUpdate()
    {
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //move the character
        move = new Vector2(direction.x, direction.y);
        rb.velocity = (move * speed * Time.deltaTime * 50);
        // change idle to run anim
        animator.SetFloat("player_speed", Mathf.Max(Mathf.Abs(rb.velocity.y), Mathf.Abs(rb.velocity.x)));
    }
}
