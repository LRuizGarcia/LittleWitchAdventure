using UnityEngine;

public class DistanceTester : MonoBehaviour
{
    public GameObject platformPrefab; // Prefab for platforms
    public GameObject playerPrefab;   // Player prefab for resetting

    public int testXDistance = 10;    // Max X distance to test
    public int testYDistance = 5;     // Max Y distance to test
    public int testDiagonalDistance = 10;

    public int platformWidth = 3;     // Width of test platforms

    private GameObject playerInstance;

    void Start()
    {
        SpawnPlayer();
        TestDiagonalDistance();

    }

    void SpawnPlayer()
    {
        // Instantiate or reset player position
        if (playerInstance == null)
        {
            playerInstance = Instantiate(playerPrefab, new Vector3(2, 3, 0), Quaternion.identity);
        }
        else
        {
            playerInstance.transform.position = new Vector3(2, 3, 0);
        }
    }

    public void TestHorizontalDistance()
    {
        ClearPlatforms();
        SpawnPlayer();

        int currentX = 0; // Start at X = 0
        for (int i = 1; i <= testXDistance; i++)
        {
            // Incrementally increase the gap between platforms
            currentX += i; // i determines the increasing distance

            // Spawn a platform at the new position
            Vector3 position = new Vector3(currentX, 0, 0); // Same height (Y = 0)
            Instantiate(platformPrefab, position, Quaternion.identity);

            Debug.Log($"Spawned platform {i} at X={currentX}");
        }
        Debug.Log("Test Horizontal Distance Complete");
    }


    public void TestVerticalDistance()
    {
        ClearPlatforms();
        SpawnPlayer();

        int currentY = 0; // Start at Y = 0
        for (int i = 1; i <= testYDistance; i++)
        {
            currentY += i; // Increment height
            Vector3 position = new Vector3(1, currentY, 0); // Same X position
            Instantiate(platformPrefab, position, Quaternion.identity);

            Debug.Log($"Spawned platform {i} at Y={currentY}");
        }
        Debug.Log("Test Vertical Distance Complete");
    }


    public void TestDiagonalDistance()
    {
        ClearPlatforms();
        SpawnPlayer();

        int currentX = 0, currentY = 0; // Start at (0, 0)
        for (int i = 1; i <= testDiagonalDistance; i++)
        {
            currentX += i+1; // Increment X
            currentY += i; // Increment Y
            Vector3 position = new Vector3(currentX, currentY, 0); // Diagonal position
            Instantiate(platformPrefab, position, Quaternion.identity);

            Debug.Log($"Spawned platform {i} at X={currentX}, Y={currentY}");
        }
        Debug.Log("Test Diagonal Distance Complete");
    }


    void ClearPlatforms()
    {
        // Clear all platforms in the scene
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");
        foreach (GameObject platform in platforms)
        {
            Destroy(platform);
        }
    }
}
