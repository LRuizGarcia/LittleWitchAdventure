using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D playerRB;
    public SpriteRenderer spriteRenderer;
    public float playerSpeed;
    public float jumpForce;
    private float input;

    //identify ground to allow jumping only when on the ground (not infinite jumping)
    public LayerMask groundLayer;
    private bool isGrounded;
    public Transform feetPosition;
    //circle around player feet to check if circle overlaps with ground
    public float groundCheckCircle;

    //to make held jump longer, but not infinite
    public float jumpTime = 0.2f;
    public float jumpTimeCounter;
    private bool isJumping;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame (variable, according to circumstances)
    void Update()
    {
        //update input value with input from user
        input = Input.GetAxisRaw("Horizontal");

        //flip character if input = left
        if(input < 0 && Time.timeScale != 0)
        {
            spriteRenderer.flipX = true;
        }

        //flip again if input = right
        else if(input > 0 && Time.timeScale != 0)
        {
            spriteRenderer.flipX = false;
        }

        //jump
        //OverlapCircle will create circle
        //feetPosition.position will put circle on player's feet
        //groundCheckCircle will make that circle into that variable
        //groundLayer will check if it overlaps with it
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, groundCheckCircle, groundLayer);

        //check if player is on the ground and if the button to jump has been pressed
        //GetButtonDown = once per press
        if (isGrounded == true && Input.GetButtonDown("Jump") && Time.timeScale != 0)
        {
            isJumping = true;
            //for every new jump, the timer for long jump is initialized
            jumpTimeCounter = jumpTime;
            //actual jump
            playerRB.linearVelocity = Vector2.up * jumpForce;
        }

        //jump higher if the jump button is held longer
        if(Input.GetButton("Jump") && isJumping == true)
        {
            //check if we ran out of jump timer
            if (jumpTimeCounter > 0)
            {
                playerRB.linearVelocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }

            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }

    //updates 50 times per second, always
    private void FixedUpdate()
    {
        //calculate direction and speed according to input (X), Y velocity is not changed
        playerRB.linearVelocity = new Vector2(input * playerSpeed, playerRB.linearVelocityY); 
    }
}
