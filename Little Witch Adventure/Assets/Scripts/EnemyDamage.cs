using UnityEngine;

public class EnemyDamage : MonoBehaviour
{

    public float damage;
    public float damageRate;
    public float knockBackForce;

    float nextDamage; //next time damage can occur

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextDamage = 0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && nextDamage < Time.time)
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.AddDamage(damage);
            nextDamage = Time.time + damageRate;

            KnockBack(collision.transform);

        }
    }

    void KnockBack(Transform knockedObject)
    {

        Vector2 knockDirection = new Vector2(knockedObject.position.x - transform.position.x, knockedObject.position.y - transform.position.y).normalized; //direction in which player will be knocked back
        knockDirection *= knockBackForce; //we add the force of the knockback to the direction

        PlayerMovement playerMovement = knockedObject.gameObject.GetComponent<PlayerMovement>();
        playerMovement.KnockBack(knockDirection); //we apply the knockback

    }


}
