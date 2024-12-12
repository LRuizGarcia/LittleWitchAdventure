using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    Rigidbody2D projectileRB;

    public float projectileSpeed;



    // Start is when everything starts
    // Awake is when this option first comes to life
    void Awake()
    {
        projectileRB = GetComponent<Rigidbody2D>();

        if (transform.localRotation.z > 0) //if we changed the rotation of the projectile (so it's facing left), move left
        {
            projectileRB.AddForce(new Vector2(-1, 0) * projectileSpeed, ForceMode2D.Impulse);
        }
        else
        {
            projectileRB.AddForce(new Vector2(1, 0) * projectileSpeed, ForceMode2D.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Stops the projectile
    public void RemoveForce()
    {
        projectileRB.linearVelocity = new Vector2(0, 0);
    }
}
