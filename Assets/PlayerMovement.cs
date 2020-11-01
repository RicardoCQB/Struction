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

    [Space]
    [Header("Collisions")]
    public float groundOffset = 0.05f;

    private bool isGrounded;
    private bool jumpRequest;

    public LayerMask playerMask;

    private Vector2 playerSize;
    private Vector2 groundedBoxSize;

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
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 dir = new Vector2(x, y);

        Walk(dir);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequest = true;
        }
    }

    private void FixedUpdate()
    {
        if (jumpRequest)
        {            
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * playerJumpForce, ForceMode2D.Impulse);
            jumpRequest = false;
            isGrounded = false;
        }
        else
        {
            Vector2 boxCenter = (Vector2)transform.position + Vector2.down * (playerSize.y + groundedBoxSize.y) * 0.5f;
            isGrounded = Physics2D.OverlapBox(boxCenter, groundedBoxSize, 0f, playerMask) != null;
        }
    }

    private void Walk(Vector2 dir)
    {
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
    }

    private void Jump(Vector2 dir)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * playerJumpForce;
    }
}
