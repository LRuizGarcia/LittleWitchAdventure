using UnityEngine;

public class FallThrough : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //to let enemies fall through objects
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Shootable"), LayerMask.NameToLayer("Shootable"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
