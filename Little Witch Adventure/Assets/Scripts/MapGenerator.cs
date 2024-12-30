using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private int gridWidth = 100;
    private int gridHeight = 20;

    private int[,][] grid;

    private int maxHorizontalGap = 4;
    private int minPlatformWidth = 3;
    private int maxPlatformWidth = 7;

    private Vector2 witchSpawnPosition = Vector2.zero;

    // Grid element IDs
    private const int EMPTY = 0;
    private const int REGULAR_PLATFORM = 1;
    private const int TALL_PLATFORM = 2;
    private const int HINGE_PLATFORM = 3;
    private const int MUSHROOM = 4;
    private const int SPIKY_BUSH = 5;
    private const int WOODEN_SPIKES = 6;
    private const int FROG = 7;
    private const int WOLF = 8;
    private const int GEM = 9;
    private const int PORTAL = 10;

    // Max allowed vertical distance (Y) for a given horizontal distance (X)
    private Dictionary<int, int> maxYForX = new Dictionary<int, int>
    {
        { 0, 2 },
        { 1, 2 },
        { 2, 2 },
        { 3, 2 },
        { 4, 2 }
    };

    private Dictionary<int, int> maxYForXWithShroom = new Dictionary<int, int>
    {
        { 0, 6 },
        { 1, 5 },
        { 2, 5 },
        { 3, 5 },
        { 4, 4 },
        { 5, 3 }
    };

    void Start()
    {
        InitializeGrid();
        CreateViablePath(); // Ensures a path from left to right
        AddExplorationPlatforms();
        InstantiateGrid();
        DebugGrid();
        PlaceWitchOnFirstPlatform();
    }

    void PlaceWitchOnFirstPlatform()
    {
        GameObject witch = GameObject.FindWithTag("Player");
        if (witch != null)
        {
            witch.transform.position = witchSpawnPosition;
            Debug.Log($"Witch positioned at {witchSpawnPosition}");
        }
        else
        {
            Debug.LogError("Witch not found. Ensure the player object has the tag 'Player'.");
        }
    }

    void InitializeGrid()
    {
        grid = new int[gridWidth, gridHeight][];

        // Initialize whole grid as EMPTY
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = new int[2] { EMPTY, EMPTY };
            }
        }

        Debug.Log("Grid initialized.");
    }

    void DebugGrid()
    {
        for (int j = gridHeight - 1; j >= 0; j--)
        {
            string row = "";
            for (int i = 0; i < gridWidth; i++)
            {
                row += grid[i, j][0] + " " + grid[i, j][1] + " ";
            }
            Debug.Log(row);
        }
    }

    void AddElementToCell(int x, int y, int element)
    {
        if (grid[x, y][0] == EMPTY)
        {
            grid[x, y][0] = element; // Place in primary slot
        }
        else if (grid[x, y][1] == EMPTY)
        {
            grid[x, y][1] = element; // Place in secondary slot
        }
        else
        {
            Debug.LogError($"Cell ({x}, {y}) already has two elements!");
        }
    }

    bool IsElementInCell(int x, int y, int element)
    {
        return (grid[x, y][0] == element || grid[x, y][1] == element);
    }

    void CreateViablePath()
    {
        int startX = 0;
        int startY = Random.Range(0, gridHeight - 3);

        int platformWidth = Random.Range(minPlatformWidth, maxPlatformWidth + 1);
        AddPlatform(startX, startY, platformWidth);

        witchSpawnPosition = new Vector2(startX + 1, startY + 2);

        RecursiveViablePath(startX + platformWidth - 1, startY);

        Debug.Log("Viable Path Created.");
    }

    void RecursiveViablePath(int lastX, int lastY)
    {
        if (lastX > gridWidth - 7)
        {
            /*AddElementToCell(lastX - 1, lastY + 1, PORTAL);
            return;*/

            if (lastX - 1 >= 0 && lastY + 1 < gridHeight)
            {
                AddElementToCell(lastX - 1, lastY + 1, PORTAL);
            }
            else
            {
                Debug.LogError($"Failed to place portal: lastX={lastX}, lastY={lastY}");
            }
            return;

        }

        else
        {
            int horizontalGap = Random.Range(1, maxHorizontalGap + 1);
            int startX = lastX + horizontalGap;

            int verticalGap;
            int startY = 0;
            bool shroom = false;

            if(Random.value < 0.5f)
            {                
                verticalGap =  maxYForXWithShroom[horizontalGap];
                startY = lastY + verticalGap;
                if(startY < gridHeight - 2)
                {
                    shroom = true;
                    AddElementToCell(lastX, lastY + 1, MUSHROOM);
                }
            }

            if (!shroom)
            {
                verticalGap = Random.Range(-maxYForX[horizontalGap], maxYForX[horizontalGap] + 1);
                startY = lastY + verticalGap;

                int safetyCounter = 0;
                while ((horizontalGap + Mathf.Abs(verticalGap) < 3 || startY > gridHeight - 2 || startY < 0) && safetyCounter < 100)
                {
                    verticalGap = Random.Range(-maxYForX[horizontalGap], maxYForX[horizontalGap] + 1);
                    startY = lastY + verticalGap;
                    safetyCounter++;
                }

                if (safetyCounter >= 100)
                {
                    Debug.LogError("Failed to find a valid verticalGap after 100 attempts. Exiting loop. Horizontal gap was " + horizontalGap);
                    return;
                }
            }
            

            int platformWidth = Random.Range(minPlatformWidth, maxPlatformWidth + 1);
            AddPlatform(startX, startY, platformWidth);

            RecursiveViablePath(startX + platformWidth - 1, startY);

        }
    }




    void AddPlatform(int startX, int y, int length)
    {
        bool isTallPlatform = Random.value < 0.25f;

        for (int i = 0; i < length; i++)
        {
            if (startX + i < gridWidth)
            {
                if (isTallPlatform)
                {
                    for (int j = y; j >= 0; j--)
                    {
                        //if (grid[startX + i, j] != EMPTY) break; // Stop if colliding with another platform
                        AddElementToCell(startX + i, j, TALL_PLATFORM);
                    }
                }
                else
                {
                    AddElementToCell(startX + i, y, REGULAR_PLATFORM);
                }                
            }
        }
    }
    void AddExplorationPlatforms()
    {
        int numExplorationPlatforms = Random.Range(10, 20); // Number of exploratory platforms

        for (int i = 0; i < numExplorationPlatforms; i++)
        {
            int startX = Random.Range(0, gridWidth);
            int startY = Random.Range(1, gridHeight - 1);

            // Check if the platform would be too close to existing ones
            if (PlatformNearby(startX, startY))
                continue;

            // Generate platform
            int platformWidth = Random.Range(minPlatformWidth, maxPlatformWidth + 1);
            AddPlatform(startX, startY, platformWidth);
        }
    }

    // Utility function to check if nearby platforms exist
    bool PlatformNearby(int x, int y, int radius = 2)
    {
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < gridWidth && ny >= 0 && ny < gridHeight)
                {
                    if (grid[nx, ny][0] != EMPTY || grid[nx, ny][1] != EMPTY)
                        return true;
                }
            }
        }
        return false;
    }



    void InstantiateGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (IsElementInCell(x, y, REGULAR_PLATFORM))
                {
                    // Check neighbors to determine the type of platform piece
                    bool isLeftEdge = (x == 0) || !IsElementInCell(x - 1, y, REGULAR_PLATFORM);
                    bool isRightEdge = (x == gridWidth - 1) || !IsElementInCell(x + 1, y, REGULAR_PLATFORM);

                    if (isLeftEdge)
                    {
                        Instantiate(Resources.Load("Prefabs/PlatformLeft"), new Vector3(x, y, 0), Quaternion.identity);
                    }
                    else if (isRightEdge)
                    {
                        Instantiate(Resources.Load("Prefabs/PlatformRight"), new Vector3(x, y, 0), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(Resources.Load("Prefabs/PlatformMiddle"), new Vector3(x, y, 0), Quaternion.identity);
                    }
                }
                if (IsElementInCell(x, y, TALL_PLATFORM))
                {
                    // Tall platform instantiation
                    bool isTop = (y + 1 >= gridHeight) || !IsElementInCell(x, y + 1, TALL_PLATFORM);// No platform above
                    bool isLeftEdge = (x == 0) || !IsElementInCell(x - 1, y, TALL_PLATFORM);
                    bool isRightEdge = (x == gridWidth - 1) || !IsElementInCell(x + 1, y, TALL_PLATFORM);

                    if (isTop)
                    {
                        if (isLeftEdge)
                        {
                            Instantiate(Resources.Load("Prefabs/TallPlatform1"), new Vector3(x, y, 0), Quaternion.identity);
                        }
                        else if (isRightEdge)
                        {
                            Instantiate(Resources.Load("Prefabs/TallPlatform3"), new Vector3(x, y, 0), Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(Resources.Load("Prefabs/TallPlatform2"), new Vector3(x, y, 0), Quaternion.identity);
                        }
                    }
                    else
                    {
                        if (isLeftEdge)
                        {
                            Instantiate(Resources.Load("Prefabs/TallPlatform4"), new Vector3(x, y, 0), Quaternion.identity);
                        }
                        else if (isRightEdge)
                        {
                            Instantiate(Resources.Load("Prefabs/TallPlatform6"), new Vector3(x, y, 0), Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(Resources.Load("Prefabs/TallPlatform5"), new Vector3(x, y, 0), Quaternion.identity);
                        }
                    }
                }
                if (IsElementInCell(x, y, MUSHROOM))
                {
                    Instantiate(Resources.Load("Prefabs/BouncyShroom"), new Vector3(x, y, 0), Quaternion.identity);
                }
                if (IsElementInCell(x, y, PORTAL))
                {
                    Instantiate(Resources.Load("Prefabs/Portal"), new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }

        Debug.Log("Grid instantiated.");
    }

}
