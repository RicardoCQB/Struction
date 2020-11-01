using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    [Space]
    [Header("Player Stats")]
    public float speed = 10;
    [Range(1, 10)]
    public float playerJumpForce = 10;

    private bool isGrounded;
    private bool jumpRequest;

    // Start is called before the first frame update
    void Start()
    {
        // Gets the components of the player Game Object        
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 dir = new Vector2(x, y);

        Walk(dir);

        if (Input.GetButtonDown("Jump"))
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
