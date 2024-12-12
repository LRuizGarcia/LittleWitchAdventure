using UnityEngine;

public class ProjectileHit : MonoBehaviour
{

    public float attackDamage;

    ProjectileController myPC;

    public GameObject explosionEffect;

    void Awake()
    {
        //el controlador del projectil est� situat al pare de IceShard
        myPC = GetComponentInParent<ProjectileController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Es crida quan el collider entra en contacte amb un altre amb Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        //hem creat un custom Layer anomenat Shootable
        //nom�s elements amb aquest Layer es veuran afectats
        //si trobem un element del layer Shootable...
        if(other.gameObject.layer == LayerMask.NameToLayer("Shootable"))
        {
            //parem el moviment del projectil
            myPC.RemoveForce();
            //cridem l'efecte d'explosi�
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
    /*
    //Si no es troba la collisi� inicial que cridaria OnTriggerEnter2D
    //tenim aquest m�tode com plan B (�s el mateix codi)
    private void OnTriggerStay2D(Collider2D other)
    {
        //hem creat un custom Layer anomenat Shootable
        //nom�s elements amb aquest Layer es veuran afectats
        //si trobem un element del layer Shootable...
        if (other.gameObject.layer == LayerMask.NameToLayer("Shootable"))
        {
            //parem el moviment del projectil
            myPC.RemoveForce();
            //cridem l'efecte d'explosi�
            Instantiate(explosionEffect, transform.position, transform.rotation);
            //destruim el projectil
            Destroy(gameObject);

            if (other.tag == "Enemy")
            {
                EnemyHealth hurtEnemy = other.gameObject.GetComponent<EnemyHealth>();
                hurtEnemy.AddDamage(attackDamage);
            }
        }
    }*/
}
