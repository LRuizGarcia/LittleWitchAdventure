using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    public float enemyMaxHealth;
    public Slider healthSlider;

    //events for enemy controller
    public event System.Action OnDamageTaken;
    public event System.Action OnDeath;

    float currentHealth;

    //Loot drops
    public bool canDropLoot;
    public GameObject loot;
    public float lootHeight;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        currentHealth = enemyMaxHealth;

        healthSlider.maxValue = enemyMaxHealth;
        healthSlider.value = currentHealth;

    }

    public void AddDamage(float damage)
    {
        if (currentHealth > 0)
        {
            healthSlider.gameObject.SetActive(true);
            currentHealth -= damage;
            healthSlider.value = currentHealth;

            OnDamageTaken?.Invoke(); //triggers OnDamageTaken event

            if (currentHealth <= 0)
            {
                MakeDead();
            }
        }
    }

    public void AddDamageFromStomp()
    {
        currentHealth = 0;
        MakeDead();
    }

    void MakeDead()
    {
        audioManager.PlaySFX(audioManager.enemyDeath);
        OnDeath?.Invoke(); //triggers OnDeath event

        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

    }

    public void OnDeathAnimationEnd()
    {
        
        if (canDropLoot)
        {
            Vector3 dropPosition = new(transform.position.x, transform.position.y + lootHeight, 0);
            Instantiate(loot, dropPosition, transform.rotation);
        }
        Destroy(transform.parent.gameObject);
    }

    public bool IsDead()
    {
        return (currentHealth <= 0);
    }
}
