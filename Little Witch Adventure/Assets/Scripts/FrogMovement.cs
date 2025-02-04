using UnityEngine;


public class FrogMovement : MonoBehaviour
{

    public GameObject projectile;
    public float shootRate;
    public Transform shootFrom;

    bool facingRight = false;

    float nextShootTime;
    Animator frogAnimator;

    EnemyHealth enemyHealth;
    Rigidbody2D enemyRB;
    Collider2D frogCollider;

    bool playerInAttackZone = false;

    void Awake()
    {
        enemyHealth = gameObject.GetComponentInChildren<EnemyHealth>();

    }

    void Start()
    {
        frogAnimator = GetComponentInChildren<Animator>();
        nextShootTime = 0f;

        enemyRB = GetComponent<Rigidbody2D>();

        //sub to events from enemyHealth
        enemyHealth.OnDamageTaken += HandleDamageTaken;
        enemyHealth.OnDeath += HandleDeath;
        
        frogCollider = GetComponent<Collider2D>();
        if (frogCollider != null)
        {
            frogCollider.enabled = false;
            Invoke("EnableCollider", 2.0f); // Enable collider after 2 seconds
        }
    }

    private void EnableCollider()
    {
        if (frogCollider != null)
        {
            frogCollider.enabled = true;
        }
    }

    void OnDestroy()
    {
        enemyHealth.OnDamageTaken -= HandleDamageTaken;
        enemyHealth.OnDeath -= HandleDeath;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInAttackZone && Time.time >= nextShootTime && !enemyHealth.IsDead()) //if player is inside the zone
        {
            nextShootTime = Time.time + shootRate;
            frogAnimator.SetTrigger("spit");
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player") && !enemyHealth.IsDead()) //if it is the player and enemy is not dead
        {
            if (facingRight && other.transform.position.x < transform.position.x) Flip(); //if facingRight but being approached from left, flip
            else if (!facingRight && other.transform.position.x > transform.position.x) Flip(); //if not facingRight but being approached from right, flip

            playerInAttackZone = true; //player entered the zone
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !enemyHealth.IsDead()) //if it is the player and enemy is not dead
        {
            if (facingRight && other.transform.position.x < transform.position.x) Flip(); //if facingRight but being approached from left, flip
            else if (!facingRight && other.transform.position.x > transform.position.x) Flip(); //if not facingRight but being approached from right, flip
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInAttackZone = false; //player left the zone
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


    public void ShootSpit()
    {
        GameObject spit = Instantiate(projectile, shootFrom.position, transform.rotation);
        SpitController spitController = spit.GetComponent<SpitController>();
        if (spitController != null)
        {
            spitController.SetDirection(facingRight ? 1 : -1); // Pass direction (+1 for right, -1 for left)
        }
    }

    void HandleDamageTaken()
    {
        if (frogAnimator != null)
        {
            frogAnimator.SetTrigger("takeDamage");
        }
    }

    void HandleDeath()
    {
        enemyRB.gravityScale = 0;
        if (frogAnimator != null)
        {
            frogAnimator.SetTrigger("die");
        }
    }
}
