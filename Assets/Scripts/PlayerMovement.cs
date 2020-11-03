using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    public Transform groundCheckPoint, groundCheckPoint2;
    public LayerMask whatIsGround;
    private bool isGrounded;

    [Space]
    [Header("Player Stats")]
    public float speed = 8;
    public float playerJumpForce = 10;
    public float hangTime = .2f; // Time for the player to jump after he leaves a platform.
    public float jumpBufferLength = 0.5f;

    private float hangCounter;
    private float jumpBufferCount;

    // Start is called before the first frame update
    void Start()
    {
        // Gets the components of the player Game Object        
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {    
        // Lets the player move horizontally
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rb.velocity.y);

        // Check if the player is on the ground / platform        
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .1f, whatIsGround)
            || Physics2D.OverlapCircle(groundCheckPoint2.position, .1f, whatIsGround);

        // If the player is grounded it has to jump in the next hangTime seconds
        if (isGrounded)
        {
            hangCounter = hangTime;
        }
        else
        {
            hangCounter -= Time.deltaTime;
        }

        // Manage Jump Buffer, that means the player can press the jump button before landing in a platform
        // and that jump will still be registered.
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCount = jumpBufferLength;
        }
        else
        {
            jumpBufferCount -= Time.deltaTime;
        }

        // Jumps if the button is pressed and it has solid ground (hangTime is positive) under the player
        if (jumpBufferCount >=0 && hangCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, playerJumpForce);
            jumpBufferCount = 0;
        }

        // If the Jump button is up then it slows the player vertical velocity
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }


}
