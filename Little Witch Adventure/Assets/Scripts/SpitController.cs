using UnityEngine;

public class SpitController : MonoBehaviour
{
    public float speed;

    Rigidbody2D spitRB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spitRB = GetComponent<Rigidbody2D>();
        if(transform.localRotation.y != 0) //if spit has spawned rotated, frog is facing left
        {
            spitRB.AddForce(new Vector2(-1, 0) * speed, ForceMode2D.Impulse); //add force to the left
        }
        else
        {
            spitRB.AddForce(new Vector2(1, 0) * speed, ForceMode2D.Impulse);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
