using UnityEngine;

public class SpitController : MonoBehaviour
{
    public float speed;

    Rigidbody2D spitRB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spitRB = GetComponent<Rigidbody2D>();

        if (transform.position.x - transform.parent.position.x < 0) //if spit spawns to the left of the frog
        {
            
            spitRB.AddForce(new Vector2(-1, 0) * speed, ForceMode2D.Impulse); //add force to the left
        }
        else
        {
            spitRB.AddForce(new Vector2(1, 0) * speed, ForceMode2D.Impulse);
        }


    }

}
