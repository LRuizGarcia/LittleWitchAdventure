using System.Threading;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;
    public float smoothSpeed = 0.9f;

    float lowY = 4.5f; //the lowest point the camera will go (when character falls)

    private void Start()
    {
        //lowY = transform.position.y; //lowest position is starting position
    }

    private void FixedUpdate()
    {
        //If player dies and is destroyed, we don't want to try to access it
        if (playerTransform == null)
        {
            return; 
        }

        //Camera movement
        Vector3 desiredPosition = playerTransform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed*Time.deltaTime);
        transform.position = smoothedPosition;

        //Stops camera if too low
        if(transform.position.y < lowY)
        {
            transform.position = new Vector3(transform.position.x, lowY, transform.position.z);
        }
    }

    //Set new player after respawn
    public void SetPlayerTransform(Transform newPlayerTransform)
    {
        playerTransform = newPlayerTransform;
    }
}
