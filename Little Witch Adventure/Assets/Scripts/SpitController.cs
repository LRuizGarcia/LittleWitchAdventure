using UnityEngine;

public class SpitController : MonoBehaviour
{
    public float speed;
    private int direction = 1;

    Rigidbody2D spitRB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spitRB = GetComponent<Rigidbody2D>();

        spitRB.AddForce(new Vector2(direction, 0) * speed, ForceMode2D.Impulse);

    }

    public void SetDirection(int dir)
    {
        direction = dir;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = (direction > 0); // Flips the sprite on the X-axis
        }
    }

}
