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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = enemyMaxHealth;

        healthSlider.maxValue = enemyMaxHealth;
        healthSlider.value = currentHealth;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddDamage(float damage)
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

    void MakeDead()
    {

        OnDeath?.Invoke(); //triggers OnDeath event

        if (canDropLoot)
        {
            Vector3 dropPosition = new(transform.position.x, transform.position.y + lootHeight, 0);
            Instantiate(loot, dropPosition, transform.rotation);
        }

    }

    public void OnDeathAnimationEnd()
    {
        Destroy(transform.parent.gameObject);
    }

    public bool IsDead()
    {
        return (currentHealth <= 0);
    }
}
