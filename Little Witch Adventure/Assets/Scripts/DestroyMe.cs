using UnityEngine;

public class DestroyMe : MonoBehaviour
{

    public float aliveTime;

    public bool destroyOnContact;

    void Awake()
    {
        Destroy(gameObject, aliveTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (destroyOnContact)
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
