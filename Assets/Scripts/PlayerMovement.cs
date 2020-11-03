using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    [Space]
    [Header("Player Stats")]
    public float speed = 8;
    public float playerJumpForce = 10;
    public float hangTime = 0.2f; // Time for the player to jump after he leaves a platform.

    [Space]
    [Header("Collisions")]
    public float groundOffset = 0.05f;

    private bool isGrounded;
    private bool jumpRequest;

    public LayerMask playerMask;

    private Vector2 playerSize;
    private Vector2 groundedBoxSize;
    
    private float hangCounter;

    // Start is called before the first frame update
    void Start()
    {
        // Gets the components of the player Game Object        
        rb = GetComponent<Rigidbody2D>();

        playerSize = GetComponent<BoxCollider2D>().size;
        groundedBoxSize = new Vector2(playerSize.x, groundOffset);
    }

    // Update is called once per frame
    void Update()
    {    
        // Lets the player move horizontally
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rb.velocity.y);

        // Check if the player is on the ground / platform
        Vector2 boxCenter = (Vector2)transform.position + Vector2.down * (playerSize.y + groundedBoxSize.y) * 0.5f;
        isGrounded = Physics2D.OverlapBox(boxCenter, groundedBoxSize, 0f, playerMask) != null;

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, playerJumpForce);
        }
    }


}
