using UnityEngine;

public class EnemyDamage : MonoBehaviour
{

    public float damage;
    public float damageRate;
    public float knockBackForce;

    float nextDamage; //next time damage can occur

    EnemyHealth enemyHealth;


    void Start()
    {
        nextDamage = 0f;
        enemyHealth = GetComponent<EnemyHealth>();

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && nextDamage < Time.time)
        {
            if (gameObject.CompareTag("Enemy"))
            {
                if (!enemyHealth.IsDead()) //We only check health if it's an enemy. We don't want the enemy to keep afflicting damage when dying animation is happening
                {
                    PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

                    // Check if the player is stomping on the enemy
                    if (playerMovement != null && playerMovement.IsStomping(this.transform))
                    {
                        enemyHealth.AddDamageFromStomp();
                        playerMovement.Bounce();
                    }

                    // If not, player takes damage on contact
                    else DamageAndKnockback(other);
                }
            }
            else if (gameObject.CompareTag("Obstacle")) //Obstacles don't have health, they always damage
            {
                DamageAndKnockback(other);
            }
        }
    }

    void DamageAndKnockback (Collider2D other)
    {
        //Damage
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        playerHealth.AddDamage(damage);
        nextDamage = Time.time + damageRate;

        //Knockback
        Transform knockedObject = other.transform;
        Vector2 knockDirection = new Vector2(knockedObject.position.x - transform.position.x, knockedObject.position.y - transform.position.y).normalized; //direction in which player will be knocked back
        knockDirection *= knockBackForce; //we add the force of the knockback to the direction

        PlayerMovement playerMovement = knockedObject.gameObject.GetComponent<PlayerMovement>();
        playerMovement.KnockBack(knockDirection); //we apply the knockback
    }






}
