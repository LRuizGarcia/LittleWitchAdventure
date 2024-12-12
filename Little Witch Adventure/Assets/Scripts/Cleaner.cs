using UnityEngine;

public class Cleaner : MonoBehaviour
{
    public event System.Action OnPlayerFall;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerFall?.Invoke();
        }
        else Destroy(other.gameObject);
    }
}
