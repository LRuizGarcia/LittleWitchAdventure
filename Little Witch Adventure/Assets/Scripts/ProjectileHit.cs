using UnityEngine;

public class ProjectileHit : MonoBehaviour
{

    public float attackDamage;

    ProjectileController myPC;

    public GameObject explosionEffect;

    AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        //el controlador del projectil está situat al pare de IceShard
        myPC = GetComponentInParent<ProjectileController>();
    }


    //Es crida quan el collider entra en contacte amb un altre amb Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        //hem creat un custom Layer anomenat Shootable
        //només elements amb aquest Layer es veuran afectats
        //si trobem un element del layer Shootable...
        if(other.gameObject.layer == LayerMask.NameToLayer("Shootable"))
        {
            audioManager.PlaySFX(audioManager.iceExplosion);
            //parem el moviment del projectil
            myPC.RemoveForce();
            //cridem l'efecte d'explosió
            Instantiate(explosionEffect, transform.position, transform.rotation);
            //destruim el projectil
            Destroy(gameObject);

            if(other.tag == "Enemy")
            {
                EnemyHealth hurtEnemy = other.gameObject.GetComponent<EnemyHealth>();
                hurtEnemy.AddDamage(attackDamage);
            }
        }
    }

}
