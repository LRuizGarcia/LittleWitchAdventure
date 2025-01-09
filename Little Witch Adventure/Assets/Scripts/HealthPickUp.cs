using UnityEngine;

public class HealthPickUp : MonoBehaviour
{

    public float healthAmount;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioManager.PlaySFX(audioManager.healthPickUp);
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.AddHealth(healthAmount);
            Destroy(gameObject);
        }
    }
}
