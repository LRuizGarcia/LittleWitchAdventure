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

    bool playerInAttackZone = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        frogAnimator = GetComponentInChildren<Animator>();
        nextShootTime = 0f;

        enemyHealth = gameObject.GetComponentInChildren<EnemyHealth>();

        //sub to events from enemyHealth
        enemyHealth.OnDamageTaken += HandleDamageTaken;
        enemyHealth.OnDeath += HandleDeath;
    }
    void OnDestroy()
    {
        enemyHealth.OnDamageTaken -= HandleDamageTaken;
        enemyHealth.OnDeath -= HandleDeath;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInAttackZone && Time.time >= nextShootTime) //if player is inside the zone
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
        float facingX = gameObject.transform.localScale.x; //find current localScale
        facingX *= -1; //invert it
        gameObject.transform.localScale = new Vector3(facingX, gameObject.transform.localScale.y, gameObject.transform.localScale.z); //update localScale
        facingRight = !facingRight; //invert bool
    }

    public void ShootSpit()
    {
        Instantiate(projectile, shootFrom.position, transform.rotation, transform);
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
        if (frogAnimator != null)
        {
            frogAnimator.SetTrigger("die");
        }
    }
}
