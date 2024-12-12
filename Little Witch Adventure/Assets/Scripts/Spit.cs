using UnityEngine;


public class Spit : MonoBehaviour
{

    public GameObject projectile;
    public float shootRate;
    public Transform shootFrom;

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
        if (other.CompareTag("Player"))
        {
            playerInAttackZone = true; //player entered the zone
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInAttackZone = false; //player left the zone
        }
    }

    public void ShootSpit()
    {
        Instantiate(projectile, shootFrom.position, transform.rotation);
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
