using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public float speed = 10f;
    public float JumpForce = 10f;

    private bool isGrounded = false;
    public Transform groundCheckCollider;
    private const float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private int doubleJump = 0;

    public LayerMask deathLayer;
    private const float deathCheckRadius = 0.5f;
    public Transform deathCheckCollider;

    private GameMaster gm;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        gm.lastCheckPointPos = Vector2.zero;
        transform.position = gm.lastCheckPointPos;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            //Debug.Log("jump");
            if (isGrounded == true || doubleJump == 0)
            {
                rb.velocity = Vector2.up * JumpForce;
                doubleJump++;
                //Debug.Log("floor");
                //rb.AddForce(new Vector2(0f,JumpForce));
            }
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
        
        float move = Input.GetAxis("Horizontal");
        float direction = Mathf.Sign(move);

        if (move != 0f)
        {
            Vector2 localScale = transform.localScale;
            localScale.x = direction;
            transform.localScale = localScale;
        }

        Vector2 velocity = rb.velocity;
        velocity.x = move * speed;
        rb.velocity = velocity;

        Collider2D[] deaths = Physics2D.OverlapCircleAll
            (deathCheckCollider.position, deathCheckRadius, deathLayer);
        if (deaths.Length > 0)
        {
            transform.position = gm.lastCheckPointPos;
            //Debug.Log("dead");
        }
    }

    void GroundCheck()
    {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll
            (groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
            doubleJump = 0;
        }
    }
}
