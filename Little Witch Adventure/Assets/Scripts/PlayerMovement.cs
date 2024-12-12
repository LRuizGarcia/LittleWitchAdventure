using System;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D playerRB;
    public SpriteRenderer spriteRenderer;
    bool facingRight;
    public float playerSpeed;
    public float jumpForce;

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

    private Animator playerAnimator;

    //for Attack
    public Transform magicOrigin;
    public GameObject iceShard;
    float fireRate = 0.5f; //time between fires
    float nextFire = 0f; //how soon after fire can fire again

    //for Knockback
    private bool isKnockedBack;
    private float knockbackDuration = 0.2f;
    private float knockbackTimer;

    bool dead = false;

    public event System.Action OnPlayerDeath;
    public event System.Action OnLevelWin;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        facingRight = true;
    }

    // Update is called once per frame (variable, according to circumstances)
    void Update()
    {
        //attack
        if (Input.GetAxisRaw("Fire1") > 0 && !dead) //Fire1 = Left ctrl or Left Mouse Button
        {
            FireIceShard();
        }
    }

    //updates 50 times per second, always
    private void FixedUpdate()
    {
        if (isKnockedBack)
        {
            //update knockback timer
            knockbackTimer -= Time.fixedDeltaTime;
            if (knockbackTimer <= 0)
            {
                isKnockedBack = false;
            }
            return; //block all movement during knockback
        }

        //update input value with input from user
        float input = Input.GetAxisRaw("Horizontal");

        playerAnimator.SetFloat("speed", Mathf.Abs(input));

        //calculate direction and speed according to input (X), Y velocity is not changed
        if (!dead)
        {
            playerRB.linearVelocity = new Vector2(input * playerSpeed, playerRB.linearVelocityY);
        }

        //flip character
        if (input < 0 && facingRight && !PauseMenu.isPaused && !dead)
        {
            Flip();
        }

        else if (input > 0 && !facingRight && !PauseMenu.isPaused && !dead)
        {
            Flip();
        }

        //jump
        //OverlapCircle will create circle
        //feetPosition.position will put circle on player's feet
        //groundCheckCircle will make that circle into that variable
        //groundLayer will check if it overlaps with it
        isGrounded = Physics2D.OverlapCircle(feetPosition.position, groundCheckCircle, groundLayer);

        //check if player is on the ground and if the button to jump has been pressed
        //GetButtonDown = once per press
        if (isGrounded && Input.GetButtonDown("Jump") && Time.timeScale != 0 && !dead)
        {
            isJumping = true;
            //for every new jump, the timer for long jump is initialized
            jumpTimeCounter = jumpTime;
            //actual jump
            playerRB.linearVelocity = Vector2.up * jumpForce;
        }

        //jump higher if the jump button is held longer
        if (Input.GetButton("Jump") && isJumping && !dead)
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
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void FireIceShard()
    {
        if (Time.time > nextFire && !dead) //if we can fire again
        {
            nextFire = Time.time + fireRate; //next time you can fire is now + the rate of fire
            if (facingRight)
            {
                //Create projectile in magicOrigin position without rotation
                Instantiate(iceShard, magicOrigin.position, Quaternion.Euler(new Vector3(0, 0, 0)));

            }
            else
            {
                //Create projectile in magicOrigin position facing left
                Instantiate(iceShard, magicOrigin.position, Quaternion.Euler(new Vector3(0, 0, 180f)));
            }
        }
    }

    public void KnockBack(Vector2 force)
    {
        isKnockedBack = true;
        knockbackTimer = knockbackDuration;
        playerRB.AddForce(force, ForceMode2D.Impulse);
    }

    public void TakeDamage()
    {
        playerAnimator.SetTrigger("takeDamage");
    }


    public void Die()
    {
        playerRB.linearVelocity = Vector2.zero;
        playerAnimator.SetTrigger("die");
        dead = true;
    }
    public void OnDeathAnimationEnd()
    {
        OnPlayerDeath?.Invoke();
        Destroy(gameObject);
    }

    public void WinGame()
    {
        playerRB.linearVelocity = Vector2.zero;
        playerAnimator.SetTrigger("win");
    }

    public void OnWinAnimationEnd()
    {
        OnLevelWin?.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            ScoreManager.instance.AddPoint();            
        }
    }
}
