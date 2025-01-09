using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{

    public float enemySpeed;

    Animator enemyAnimator;

    //Facing direction
    public GameObject enemyGraphic; //the GameObject where the sprite is located (child of this GameObject)
    bool canFlip = true; //enemy can only flip if not attacking
    bool facingRight = false;
    float flipTime = 5f; //how often enemy can flip during idle time
    float nextFlipChance = 0f; //next time that the enemy might flip

    //Attacking
    public float attackTime; //how long since player enters attack zone until it can attack
    float startAttackTime; //?
    bool isAttacking;
    Rigidbody2D enemyRB;
    Collider2D wolfCollider;
    EnemyHealth enemyHealth;

    void Awake()
    {
        enemyHealth = gameObject.GetComponentInChildren<EnemyHealth>();

    }


    void Start()
    {
        enemyAnimator = GetComponentInChildren<Animator>();
        enemyRB = GetComponent<Rigidbody2D>();

        //sub to events from enemyHealth
        enemyHealth.OnDamageTaken += HandleDamageTaken;
        enemyHealth.OnDeath += HandleDeath;

        wolfCollider = GetComponent<Collider2D>();
        if (wolfCollider != null)
        {
            wolfCollider.enabled = false;
            Invoke("EnableCollider", 2.0f); // Enable collider after 2 seconds
        }
    }

    private void EnableCollider()
    {
        if (wolfCollider != null)
        {
            wolfCollider.enabled = true;
        }
    }

    void OnDestroy()
    {
        enemyHealth.OnDamageTaken -= HandleDamageTaken;
        enemyHealth.OnDeath -= HandleDeath;
    }

    void Update()
    {
        //Enemy random flip during idle        
        if (Time.time > nextFlipChance && !enemyHealth.IsDead())//If it's passed enough time
        {

            if (canFlip && Random.Range(0, 10) >= 5) Flip();//Random chance to flip (right now 50%)
            nextFlipChance = Time.time + flipTime;//update next flip chance time

        }

    }

    //When Player ENTERS Attack Zone
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && !enemyHealth.IsDead()) //if it is the player and enemy is not dead
        {
            if (facingRight && other.transform.position.x < transform.position.x) Flip(); //if facingRight but being approached from left, flip
            else if (!facingRight && other.transform.position.x > transform.position.x) Flip(); //if not facingRight but being approached from right, flip

            canFlip = false; //once player is in Attack Zone, enemy must stop flipping randomly
            isAttacking = true; //is about to attack
            startAttackTime = Time.time + attackTime; //attack will start attackTime seconds from now, giving player some time
        }
    }

    //When Player STAYS in Attack Zone
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !enemyHealth.IsDead()) //if it is the player and enemy is not dead
        {
            if (!isAttacking) //sometimes OnTriggerEnter2D is skipped, so we copy that code here to make sure it is used
            {
                if (facingRight && other.transform.position.x < transform.position.x) Flip();
                else if (!facingRight && other.transform.position.x > transform.position.x) Flip();

                canFlip = false;
                isAttacking = true;
                startAttackTime = Time.time + attackTime;
            }

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>(); //we get the PlayerHealth to check if alive, so it stops attacking when dead
            if (!playerHealth.IsDead() && startAttackTime < Time.time) //if player is not dead and if it's time to attack
            {
                //attack animation
                enemyAnimator.SetBool("isAttacking", isAttacking);
                //initial movement forward
                if (facingRight) enemyRB.AddForce(new Vector2(1, 0) * enemySpeed);
                else enemyRB.AddForce(new Vector2(-1, 0) * enemySpeed);

            }
        }
    }

    //When Player LEAVES Attack Zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) //if it is the player
        {
            canFlip = true; //can start flipping randomly again
            isAttacking = false;
            enemyRB.linearVelocity = Vector2.zero; //stop moving
            //idle animation
            enemyAnimator.SetBool("isAttacking", isAttacking);

        }
    }


    void Flip()
    {
        if (!enemyHealth.IsDead())
        {
            // Flip the rotation by modifying the Y axis of the parent GameObject
            float newRotationY = transform.eulerAngles.y == 0 ? 180f : 0f;
            transform.rotation = Quaternion.Euler(0f, newRotationY, 0f);

            facingRight = !facingRight; // Invert bool
        }
    }


    void HandleDamageTaken()
    {
        enemyRB.linearVelocity = Vector2.zero;
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("takeDamage");
        }
    }


    void HandleDeath()
    {
        enemyRB.linearVelocity = Vector2.zero;
        enemyRB.gravityScale = 0;
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("die");
        }
    }

}
