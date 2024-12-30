using UnityEngine;

public class GridManager : MonoBehaviour
{

    private int gridWidth = 100;
    private int gridHeight = 10;

    private int[,] grid;

    public GameObject platformPrefab;
    public GameObject platformLeftPrefab;
    public GameObject platformMiddlePrefab;
    public GameObject platformRightPrefab;

    public int numberOfPlatforms = 10;

    public int minPlatformWidth = 3;
    public int maxPlatformWidth = 7;

    public int maxVerticalGap = 2;

    public int minHorizontalGap = 3;
    public int maxHorizontalGap = 5;

    public float maxDiagonalGap = 1f;    

    void Start()
    {

        Instantiate(Resources.Load("Prefabs/BlueGem"), new Vector3(4, 4, 0), Quaternion.identity);
        grid = new int[gridWidth, gridHeight];

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                grid[i, j] = 0; 
            }
        }
        //AddGroundLayer();
        GeneratePlatforms();
        GenerateMap();
        


        DebugGrid();
    }

    void DebugGrid()
    {
        for (int j = gridHeight - 1; j >= 0; j--) 
        {
            string row = "";
            for (int i = 0; i < gridWidth; i++)
            {
                row += grid[i, j] + " ";
            }
            Debug.Log(row);
        }
    }

    void AddGroundLayer()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            grid[i, 0] = 1;
        }


        //TOP ROW
        //for (int i = 0; i < gridWidth; i++)
        //{
        //    grid[i, gridHeight-1] = 1; // 1 represents ground tiles
        //}

    }

    void GenerateMap()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if (grid[i, j] == 1) // Ground tile
                {
                    Instantiate(platformPrefab, new Vector3(i, j, 0), Quaternion.identity);
                }
                /*else if (grid[i, j] == 2) // Platform tile
                {
                    Instantiate(platformPrefab, new Vector3(i, j, 0), Quaternion.identity);
                }*/
            }
        }
    }

    //void GeneratePlatforms()
    //{
    //    int lastX = 0; // Track the last X position
    //    int lastY = 0; // Track the last Y position

    //    for (int i = 0; i < numberOfPlatforms; i++)
    //    {
    //        int startX;
    //        int startY;

    //        if (i == 0)
    //        {
    //            // Always place the first platform at the starting position
    //            startX = 0;
    //            startY = 0;
    //        }
    //        else
    //        {
    //            // Generate a random start position with rules
    //            startX = lastX + Random.Range(minHorizontalGap, maxHorizontalGap + 1); // Ensure minimum horizontal distance

    //            startY = lastY + Random.Range(-maxVerticalGap, maxVerticalGap + 1); // Random vertical variation

    //            // Ensure startY stays within bounds
    //            startY = Mathf.Clamp(startY, 1, gridHeight - 1);

    //            // Prevent adjacent platforms by skipping any overlap
    //            while (startX <= lastX + minHorizontalGap)
    //            {
    //                startX++;
    //            }

    //            // Validate diagonal distance
    //            float diagonalDistance = Mathf.Sqrt(Mathf.Pow(startX - lastX, 2) + Mathf.Pow(startY - lastY, 2));
    //            if (diagonalDistance > maxDiagonalGap)
    //            {
    //                Debug.Log($"Skipping platform {i}: Exceeds diagonal distance. Diagonal={diagonalDistance}");
    //                continue; // Try the next platform
    //            }

    //            // Validate horizontal range
    //            if (startX + minPlatformWidth > gridWidth)
    //            {
    //                Debug.LogWarning($"Skipping platform {i} because it would exceed grid bounds: StartX={startX}");
    //                break; // No more platforms can fit
    //            }
    //        }

    //        // Generate platform length
    //        int platformWidth = Random.Range(minPlatformWidth, maxPlatformWidth + 1);

    //        // Adjust width to prevent exceeding grid bounds
    //        platformWidth = Mathf.Min(platformWidth, gridWidth - startX);

    //        // Spawn platform
    //        AddPlatform(startX, startY, platformWidth);

    //        // Update lastX and lastY
    //        lastX = startX + platformWidth - 1;
    //        lastY = startY;
    //    }
    //}

    void GeneratePlatforms()
    {
        int lastX = 0; // Track the last X position
        int lastY = 0; // Track the last Y position

        for (int i = 0; i < numberOfPlatforms; i++)
        {
            int startX = 0;
            int startY = 0;

            //if (i == 0)
            //{
            //    // Always place the first platform at the starting position
            //    startX = 0;
            //    startY = 0;

            //    Debug.Log($"Platform {i}: StartX={startX}, StartY={startY}");
            //}
            if (i != 0)
            {
                // Step 1: Randomize X Position
                startX = lastX + Random.Range(minHorizontalGap, maxHorizontalGap + 1);

                // Step 2: Randomize Y Position until Valid
                bool isValidY = false;
                while (!isValidY)
                {
                    // Get a random Y value within constraints
                    startY = lastY + Random.Range(-maxVerticalGap, maxVerticalGap + 1);

                    // Ensure Y stays within bounds
                    startY = Mathf.Clamp(startY, 1, gridHeight - 1);

                    // Calculate the diagonal distance
                    float deltaX = startX - lastX;
                    float deltaY = startY - lastY;
                    float diagonalDistance = Mathf.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
                    Debug.Log($"Platform {i}: StartX={startX}, StartY={startY}, diagonal distance{diagonalDistance}");
                    // Validate diagonal distance
                    if (diagonalDistance <= maxDiagonalGap)
                    {
                        isValidY = true; // This Y is valid
                    }
                    else
                    {
                        Debug.LogWarning($"Rejected Y for Platform {i}: DeltaX={deltaX}, DeltaY={deltaY}, Diagonal={diagonalDistance}");
                    }
                }

                Debug.Log($"Platform {i}: StartX={startX}, StartY={startY}");
            }

            // Step 3: Generate Platform Length
            int platformWidth = Random.Range(minPlatformWidth, maxPlatformWidth + 1);

            // Ensure platform fits within grid bounds
            platformWidth = Mathf.Min(platformWidth, gridWidth - startX);

            // Spawn the platform
            AddPlatform(startX, startY, platformWidth);

            // Update lastX and lastY for the next platform
            lastX = startX + platformWidth - 1;
            lastY = startY;
        }
    }



    void AddPlatform(int startX, int y, int length)
    {

        // Validate starting position
        if (startX < 0 || startX + length > gridWidth || y < 0 || y >= gridHeight)
        {
            Debug.LogError($"Invalid platform position: StartX={startX}, Y={y}, Length={length}");
            return; // Skip this platform
        }

        // Instantiate the left edge
        Instantiate(platformLeftPrefab, new Vector3(startX, y, 0), Quaternion.identity);

        // Instantiate the middle pieces
        for (int i = 1; i < length - 1; i++)
        {
            Instantiate(platformMiddlePrefab, new Vector3(startX + i, y, 0), Quaternion.identity);
        }

        // Instantiate the right edge
        if (length > 1)
        {
            Instantiate(platformRightPrefab, new Vector3(startX + length - 1, y, 0), Quaternion.identity);
        }

        // Update the grid to mark these cells as occupied
        for (int i = 0; i < length; i++)
        {
            if (startX + i < gridWidth && y < gridHeight) // Ensure within bounds
            {
                grid[startX + i, y] = 2; // 2 represents platform tiles
            }
        }
        
    }

    
}
