using UnityEngine;

public class FixHealthBarFlip : MonoBehaviour
{
    private Quaternion initialRotation;

    void Start()
    {
        // Initial rotation of the health bar
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // Keep the health bar's rotation fixed, independent of the parent's rotation
        transform.rotation = initialRotation;
    }
}

