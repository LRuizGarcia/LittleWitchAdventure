using UnityEngine;

public class HealthPickUp : MonoBehaviour
{

    public float healthAmount;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.AddHealth(healthAmount);
            Destroy(gameObject);
        }
    }
}