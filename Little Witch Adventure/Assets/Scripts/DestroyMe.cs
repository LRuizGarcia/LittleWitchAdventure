using UnityEngine;

public class DestroyMe : MonoBehaviour
{

    public float aliveTime;

    public bool destroyOnContact;

    void Awake()
    {
        Destroy(gameObject, aliveTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (destroyOnContact)
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
