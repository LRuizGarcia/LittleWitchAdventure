using UnityEngine;

public class PortalController : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioManager.PlaySFX(audioManager.portal);
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            playerMovement.WinGame();
        }
    }

}
