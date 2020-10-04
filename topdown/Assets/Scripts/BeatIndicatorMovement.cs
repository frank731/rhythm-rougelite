using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatIndicatorMovement : MonoBehaviour
{
    public float movementSpeed;
    public Rigidbody2D rb;
    void Update()
    {
        rb.velocity = transform.right * movementSpeed * -1;
    }
}
