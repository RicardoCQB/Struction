using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer playerSprite;
    public Animator playerAnimator;
    public ParticleSystem footstepDust;
    public ParticleSystem.EmissionModule footstepDustEmission;
    public Transform respawnPoint;

    public Transform groundCheckPoint, groundCheckPoint2;
    public Transform leftWallCheckPoint, rightWallCheckPoint;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    private bool isGrounded;
    private bool isOnRightWall;
    private bool isOnLeftWall;
    private bool wallGrab;
    private bool hasDashed = false;

    public Transform camTarget;
    public float camAheadAmount, camAheadSpeed; // The camera moves ahead of the player by this amount when he moves.


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

        footstepDustEmission = footstepDust.emission;
    }

    // Update is called once per frame
    void Update()
    {
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        // Lets the player move horizontally
        rb.velocity = new Vector2(xRaw * speed, rb.velocity.y);
        playerAnimator.SetFloat("Speed", Mathf.Abs(xRaw));

        // Check if the player is on the ground / platform        
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .1f, whatIsGround)
            || Physics2D.OverlapCircle(groundCheckPoint2.position, .1f, whatIsGround);

        // Check if the player is on the wall  
        isOnLeftWall = Physics2D.OverlapCircle(leftWallCheckPoint.position, .5f, whatIsWall);
        isOnRightWall = Physics2D.OverlapCircle(rightWallCheckPoint.position, .5f, whatIsWall);

        // If the player is on the wall and presses the Grab it grabs the wall
        if ((isOnLeftWall || isOnRightWall) && Input.GetKey(KeyCode.Z))
            wallGrab = true;

        // If the player is not on a wall or is not pressing the Grab button it doesn't grab a wall
        if ((!isOnLeftWall && !isOnRightWall) || Input.GetKeyUp(KeyCode.Z))
            wallGrab = false;

        if (wallGrab)
            rb.velocity = new Vector2(rb.velocity.x, yRaw * (speed / 2));

        // If the player is grounded it has to jump in the next hangTime seconds
        if (isGrounded)
            hangCounter = hangTime;
        else
            hangCounter -= Time.deltaTime;

        // If the player is on the ground or left the ground recently it resets the dash
        if (isGrounded)
        {
            hasDashed = false;
            playerAnimator.SetBool("isJumping", false);
        }

        // Manage Jump Buffer, that means the player can press the jump button before landing in a platform
        // and that jump will still be registered.
        if (Input.GetKeyDown(KeyCode.C))
            jumpBufferCount = jumpBufferLength;
        else
            jumpBufferCount -= Time.deltaTime;

        // Jumps if the button is pressed and it has solid ground (hangTime is positive) under the player
        if (jumpBufferCount >= 0 && hangCounter > 0f)
        {
            playerAnimator.SetBool("isJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, playerJumpForce);
            jumpBufferCount = 0;
        }

        // If the Jump button is up then it slows the player vertical velocity
        if (Input.GetKeyUp(KeyCode.C) && rb.velocity.y > 0)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

        // If the player hasn't dashed and presses the dash button, then the player dashes
        if (Input.GetKey(KeyCode.X) && !hasDashed)
        {
            if (xRaw != 0 || yRaw != 0)
            {
                hasDashed = true;
                rb.velocity = Vector2.zero;
                rb.velocity += new Vector2(xRaw, yRaw).normalized * 30;
                StartCoroutine(DashWait());
            }
        }

        //Flip the player sprite
        if (xRaw >= 0)
            playerSprite.flipX = false;
        else
            playerSprite.flipX = true;

        // Move camera
        if (xRaw != 0)
        {
            camTarget.localPosition = new Vector3(
                Mathf.Lerp(camTarget.localPosition.x, camAheadAmount * xRaw, camAheadSpeed * Time.deltaTime)
                , camTarget.localPosition.y, camTarget.localPosition.z);
        }

        IEnumerator DashWait()
        {
            StartCoroutine(GroundDash());
            rb.drag = 10;
            rb.gravityScale = 0;
            yield return new WaitForSeconds(.3f);
            rb.drag = 0;
            rb.gravityScale = 3;
        }

        IEnumerator GroundDash()
        {
            yield return new WaitForSeconds(.15f);
            if (isGrounded)
                hasDashed = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player touches the sea, it respawns in the respawn point
        if (collision.gameObject.CompareTag("Sea"))
        {
            transform.position = respawnPoint.position;
        }

        // If the player touches the sea, it respawns in the respawn point
        if (collision.gameObject.CompareTag("Finish"))
        {

            SceneManager.LoadScene("GameMenu");
            
        }
    }
}
    
