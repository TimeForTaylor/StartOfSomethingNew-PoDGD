using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toast : MonoBehaviour
{
    private Rigidbody2D rb;
    public float JumpForce = 10f;
    private bool isGrounded = false;
    public Transform groundCheckCollider;
    private const float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (isGrounded == true) 
        { 
            rb.velocity = Vector2.up * JumpForce; 
            //Debug.Log("floor");
            //////rb.AddForce(new Vector2(0f,JumpForce));
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }
    
    void GroundCheck()
    {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
        }
    }
}
