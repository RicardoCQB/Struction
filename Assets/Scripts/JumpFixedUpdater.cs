using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFixedUpdater : MonoBehaviour
{
    public float fallMultiplier = 8f;
    public float lowFallMultiplier = 6f;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallMultiplier;
        } 
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.gravityScale = lowFallMultiplier;
        }
        else
        {
            rb.gravityScale = 1;
        }
    }
}
