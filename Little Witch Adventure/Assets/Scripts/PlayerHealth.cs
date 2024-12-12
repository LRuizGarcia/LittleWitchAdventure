using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public float fullHealth;

    float currentHealth;

    PlayerMovement playerMovement;

    //HUD Variables
    public Slider healthSlider;

    public Cleaner cleaner;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = fullHealth;

        playerMovement = GetComponent<PlayerMovement>();

        //HUD Initialization
        healthSlider.maxValue = fullHealth;
        healthSlider.value = fullHealth;

        cleaner.OnPlayerFall += HandleFall;
    }

    void OnDestroy()
    {
        cleaner.OnPlayerFall -= HandleFall;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddDamage(float damage)
    {
        if (damage <= 0)
        {
            return;
        }

        playerMovement.TakeDamage();

        currentHealth -= damage;

        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            MakeDead();
        }
    }

    public void AddHealth(float healthAmount)
    {
        if (currentHealth > 0) //if player is NOT DEAD
        {
            currentHealth += healthAmount; //add health
            if (currentHealth > fullHealth) currentHealth = fullHealth; //if new health is more than full, change to full
            healthSlider.value = currentHealth; //update slider
        }
    }

    public void MakeDead()
    {
        playerMovement.Die();
    }

    public bool IsDead()
    {
        return (currentHealth <= 0);
    }

    void HandleFall()
    {
        AddDamage(currentHealth);
    }
}
