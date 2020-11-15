using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 3f;
    public float speed;
    public Rigidbody2D rb;
    public Animator animator;
    private Vector2 move;
    private Vector2 direction;
    private PlayerController playerController;
    private void Awake()
    {
        speed = baseSpeed;
        playerController = GetComponent<PlayerController>();
        playerController.resetStats.AddListener(ResetStats);    
    }

    private void ResetStats()
    {
        speed = baseSpeed;
    }

    private void FixedUpdate()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //move the character
        move = new Vector2(direction.x, direction.y);
        rb.velocity = (move * speed * Time.deltaTime * 50);
        // change idle to run anim
        animator.SetFloat("player_speed", Mathf.Max(Mathf.Abs(rb.velocity.y), Mathf.Abs(rb.velocity.x)));
    }
}
