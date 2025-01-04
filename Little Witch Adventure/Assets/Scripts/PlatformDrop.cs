using System.Collections;
using UnityEngine;

public class PlatformDrop : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(FallTimer());
        }
       
    }

    IEnumerator FallTimer()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        GetComponent<CapsuleCollider2D>().enabled = true;
    }
}
