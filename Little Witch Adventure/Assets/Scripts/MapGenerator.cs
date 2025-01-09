using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private int gridWidth = 100;
    private int gridHeight = 12;

    private int[,][] grid;

    private int maxHorizontalGap = 4;
    private int minPlatformWidth = 3;
    private int maxPlatformWidth = 7;

    GameObject witch;
    private Vector2 witchSpawnPosition = Vector2.zero;
    private Vector2 portalPosition = Vector2.zero;

    private PlatformData firstPlatform;
    private PlatformData finalPlatform;
    private List<PlatformData> platforms;
    private List<Vector2> mushrooms;


    // Grid element IDs
    private const int EMPTY = 0;
    private const int REGULAR_PLATFORM = 1;
    private const int TALL_PLATFORM = 2;
    private const int TALL_PLATFORM_BODY = 12;
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
        { 0, 5 },
        { 1, 5 },
        { 2, 5 },
        { 3, 5 },
        { 4, 4 },
        { 5, 3 }
    };

    void Start()
    {
        witch = GameObject.FindWithTag("Player");

        if (GameController.gameController.currentGeneratedLevelSeed != "")
        {

            GenerateNewLevel(int.Parse(GameController.gameController.currentGeneratedLevelSeed));
        }
        else
        {
;
            int seed = GenerateRandomSeed();            
            GenerateNewLevel(seed);
        }

    }

    public void RestartLevel()
    {
        GameController.gameController.LoadGeneratedLevel(true);    
    }

    public void GenerateNewLevel(int seed)
    {
        platforms = new List<PlatformData>();
        mushrooms = new List<Vector2>();

        SetSeed(seed);

        InitializeGrid();
        CreateViablePath();
        AddExplorationPlatforms();
        PopulatePlatforms();
        InstantiateGrid();
        DebugGrid();
        PlaceWitchOnFirstPlatform();
        SaveGridState(); // Save grid for potential restarts
        SavePlayerState(witchSpawnPosition);
    }

    int GenerateRandomSeed()
    {
        return System.DateTime.Now.GetHashCode(); // Use the current time for randomness
    }

    void SetSeed(int seed)
    {
        Random.InitState(seed); // Set the seed for Unity's random number generator
        Debug.Log($"Seed set to: {seed}");
        GameController.gameController.StartGeneratedLevel(seed.ToString());
    }

    void PlaceWitchOnFirstPlatform()
    {
        
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
            if (element == MUSHROOM) mushrooms.Add(new Vector2(x, y));
        }
        else if (grid[x, y][0] != element && grid[x, y][1] == EMPTY)
        {
            grid[x, y][1] = element; // Place in secondary slot
            if (element == MUSHROOM) mushrooms.Add(new Vector2(x, y));
        }
    }

    bool IsElementInCell(int x, int y, int element)
    {
        return (grid[x, y][0] == element || grid[x, y][1] == element);
    }

    bool IsCellEmpty(int x, int y)
    {
        return (grid[x, y][0] == EMPTY && grid[x, y][1] == EMPTY);
    }

    void CreateViablePath()
    {
        int startX = 0;
        int startY = Random.Range(0, gridHeight / 2);

        int platformWidth = Random.Range(minPlatformWidth, maxPlatformWidth + 1);

        firstPlatform = new PlatformData(startX, startY, platformWidth);

        AddPlatform(firstPlatform);        

        witchSpawnPosition = new Vector2(startX + 1, startY + 1);

        RecursiveViablePath(firstPlatform);

        Debug.Log("Viable Path Created.");
    }

    void RecursiveViablePath(PlatformData lastPlatform)
    {
        int lastX = lastPlatform.startX + lastPlatform.width - 1;
        int lastY = lastPlatform.y;

        if (lastX > gridWidth - 7)
        {

            if (lastX - 1 >= 0 && lastY + 1 < gridHeight)
            {
                AddElementToCell(lastX - 1, lastY + 1, PORTAL);
                portalPosition = new Vector2(lastX - 1, lastY + 1);
                platforms.RemoveAt(platforms.Count - 1);
                finalPlatform = lastPlatform;
            }
            else
            {
                Debug.Log($"Failed to place portal: lastX={lastX}, lastY={lastY}");
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

            if (Random.value < 0.5f)
            {
                verticalGap = maxYForXWithShroom[horizontalGap];
                startY = lastY + verticalGap;
                if (startY < gridHeight - 2)
                {
                    shroom = true;
                    AddElementToCell(lastX, lastY + 1, MUSHROOM);
                }
            }

            if (!shroom)
            {
                int jumpUpOrDown = Random.Range(0, 2) * 2 - 1; //gives 1 or -1
                verticalGap = jumpUpOrDown * maxYForX[horizontalGap];


                startY = lastY + verticalGap;

                int safetyCounter = 0;
                while ((horizontalGap + Mathf.Abs(verticalGap) < 3 || startY > gridHeight - 2 || startY < 0) && safetyCounter < 100)
                {

                    jumpUpOrDown = Random.Range(0, 2) * 2 - 1; //gives 1 or -1
                    verticalGap = jumpUpOrDown * maxYForX[horizontalGap];

                    startY = lastY + verticalGap;
                    safetyCounter++;
                }

                if (safetyCounter >= 100)
                {
                    Debug.Log("Failed to find a valid verticalGap after 100 attempts. Exiting loop. Horizontal gap was " + horizontalGap);
                    return;
                }
            }


            int platformWidth = Random.Range(minPlatformWidth, maxPlatformWidth + 1);

            if (startX + platformWidth >= gridWidth)
            {
                platformWidth = gridWidth - startX;
            }

            PlatformData newPlatform = new PlatformData(startX, startY, platformWidth);

            AddPlatform(newPlatform);

            platforms.Add(newPlatform);

            RecursiveViablePath(newPlatform);

        }
    }
  
    void AddPlatform(PlatformData platform)
    {
        bool isTallPlatform = Random.value < 0.25f;

        for (int i = 0; i < platform.width; i++)
        {
            if (platform.startX + i < gridWidth)
            {
                if (isTallPlatform)
                {
                    AddElementToCell(platform.startX + i, platform.y, TALL_PLATFORM);
                    for (int j = platform.y - 1; j >= 0; j--)
                    {
                        AddElementToCell(platform.startX + i, j, TALL_PLATFORM_BODY);
                    }
                }
                else
                {
                    AddElementToCell(platform.startX + i, platform.y, REGULAR_PLATFORM);
                }
            }
        }
    }

    void AddExplorationPlatforms()
    {
        int numExplorationPlatforms = 50; // Number of exploratory platforms

        for (int i = 0; i < numExplorationPlatforms; i++)
        {
            int startX = Random.Range(0, gridWidth);
            int startY = Random.Range(1, gridHeight - 1);

            int platformWidth = Random.Range(minPlatformWidth, maxPlatformWidth + 1);

            // Check if the platform is further on than the final portal
            if (startX + platformWidth >= portalPosition.x)
            {
                platformWidth = (int)(portalPosition.x - startX);
                if (platformWidth < minPlatformWidth) continue;
            }

            PlatformData newPlatform = new PlatformData(startX, startY, platformWidth);

            // Check if the platform would be too close to existing ones
            if (PlatformNearby(newPlatform)) continue;

            // Check if the platform is reachable from other platforms
            if (!IsPlatformReachable(newPlatform)) continue;

            AddPlatform(newPlatform);
            platforms.Add(newPlatform);
        }
    }

    bool PlatformNearby(PlatformData platform)
    {
        int radius = 1;

        for (int dx = -radius; dx <= radius + platform.width; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                int nx = platform.startX + dx;
                int ny = platform.y + dy;

                if (nx >= 0 && nx < gridWidth && ny >= 0 && ny < gridHeight)
                {
                    if (IsElementInCell(nx, ny, REGULAR_PLATFORM) || IsElementInCell(nx, ny, TALL_PLATFORM))
                        return true;
                }
            }
        }
        return false;
    }


    bool IsPlatformReachable(PlatformData platform)
    {
        int maxXDistance = 5; // Maximum horizontal distance for jumps with mushrooms
        int maxYDistance = 5; // Maximum vertical distance for jumps with mushrooms

        int minYSpacingAboveMushroom = 3; // Minimum vertical space required above a mushroom
        bool reachableWithExtraShroom = false;
        int shroomX = 0;
        int shroomY = 0;

        // Ensure no mushroom is right below platform
        for (int i = platform.startX; i < platform.startX + platform.width; i++)
        {
            if (i > gridWidth) return false;
            else if (platform.y > 0 && IsElementInCell(i, platform.y - 1, MUSHROOM)) return false;
        }

        for (int dx = -maxXDistance; dx <= maxXDistance + platform.width; dx++)
        {
            for (int dy = -maxYDistance; dy <= maxYDistance; dy++)
            {
                int neighborX = platform.startX + dx;
                int neighborY = platform.y + dy;

                // Ensure we stay within the grid bounds
                if (neighborX >= 0 && neighborX < gridWidth && neighborY >= 0 && neighborY < gridHeight)
                {
                    // Check if the current neighbor cell contains a platform
                    if (IsElementInCell(neighborX, neighborY, REGULAR_PLATFORM) || IsElementInCell(neighborX, neighborY, TALL_PLATFORM))
                    {
                        int distanceX = Mathf.Abs(platform.startX - neighborX);
                        int distanceY = Mathf.Abs(platform.y - neighborY);

                        // Check normal jump constraints
                        if (distanceX <= 4 && distanceY <= 2)
                        {
                              return true; // Platform is reachable directly
                        }

                        // Check if there's a reachable lower platform with a mushroom
                        else if (platform.y >= neighborY && IsElementInCell(neighborX, neighborY + 1, MUSHROOM))
                        {
                            if (distanceX <= 5 && distanceY <= maxYForXWithShroom[distanceX] &&
                                distanceY >= minYSpacingAboveMushroom) // Ensure enough space above
                            {
                                return true; // Reachable via existing mushroom
                            }
                        }

                        // Check about adding a mushroom to reach an upper platform AS LAST RESORT
                        else if (platform.y <= neighborY &&
                                 distanceX <= 5 &&
                                 distanceY <= maxYForXWithShroom[distanceX] &&
                                 distanceY >= minYSpacingAboveMushroom)
                        {
                            shroomX = platform.startX;
                            shroomY = platform.y + 1;


                            reachableWithExtraShroom = true;

                        }
                    }
                }
            }
        }
        if (reachableWithExtraShroom) AddElementToCell(shroomX, shroomY, MUSHROOM);
        return reachableWithExtraShroom; 
    }

    void PopulatePlatforms()
    {
        // Starting Platform (where witch spawns)
        for (int i = firstPlatform.startX + 3; i <= firstPlatform.startX + firstPlatform.width - 1; i = i + 2)
        {
            if (IsCellEmpty(i, firstPlatform.y + 1) && IsCellEmpty(i + 1, firstPlatform.y + 1)) {
                AddElementToCell(i, firstPlatform.y + 1, GEM);
            }
        }

        // Final Platform (where portal spawns)
        for (int i = finalPlatform.startX + finalPlatform.width - 4; i >= finalPlatform.startX; i = i - 2)
        {
            AddElementToCell(i, finalPlatform.y + 1, GEM);
        }

        // Rest of platforms
        foreach (PlatformData platform in platforms)
        {
            if (platform.width < 5)
            {
                float percentage = Random.value;
                if (percentage < 0.35f)
                {
                    AddElementToCell(platform.startX + platform.width - 2, platform.y + 1, FROG);
                }
                else if (percentage < 0.60f)
                {
                    AddElementToCell(platform.startX + platform.width - 2, platform.y + 1, SPIKY_BUSH);
                }
                else if (percentage < 0.85f)
                {
                    AddElementToCell(platform.startX + platform.width - 2, platform.y + 1, WOODEN_SPIKES);
                }
                for (int i = platform.startX; i < platform.startX + platform.width; i = i + 2)
                {
                    if (IsCellEmpty(i, platform.y + 1) || 
                        (IsElementInCell(i, platform.y + 1, TALL_PLATFORM_BODY) &&
                        IsElementInCell(i, platform.y + 1, EMPTY))) 
                    {
                        AddElementToCell(i, platform.y + 1, GEM);
                    }
                }
            }
            if (platform.width >= 5 && platform.width < 7)
            {
                float percentage = Random.value;
                int position = Random.Range(2, 4);
                if (percentage < 0.25f)
                {
                    AddElementToCell(platform.startX + platform.width - position, platform.y + 1, WOLF);
                }
                else if (percentage < 0.35f)
                {
                    AddElementToCell(platform.startX + platform.width - position, platform.y + 1, FROG);
                }
                else if (percentage < 0.50f)
                {
                    AddElementToCell(platform.startX + platform.width - position, platform.y + 1, SPIKY_BUSH);
                }
                else if (percentage < 0.85f)
                {
                    AddElementToCell(platform.startX + platform.width - position, platform.y + 1, WOODEN_SPIKES);
                }
                position = Random.Range(0, 2);
                for (int i = platform.startX + position; i < platform.startX + platform.width; i = i + 2)
                {
                    if (IsCellEmpty(i, platform.y + 1) ||
                        (IsElementInCell(i, platform.y + 1, TALL_PLATFORM_BODY) &&
                        IsElementInCell(i, platform.y + 1, EMPTY)))
                    {
                        AddElementToCell(i, platform.y + 1, GEM);
                    }
                }

            }
            if (platform.width >= 7)
            {
                float percentageL = Random.value;
                float percentageR = Random.value;
                int positionL = Random.Range(1, 3);
                int positionR = Random.Range(2, 4);

                if (percentageL < 0.25f)
                {
                    AddElementToCell(platform.startX + positionL, platform.y + 1, WOLF);
                }
                else if (percentageL < 0.35f)
                {
                    AddElementToCell(platform.startX + positionL, platform.y + 1, FROG);
                }
                else if (percentageL < 0.50f)
                {
                    AddElementToCell(platform.startX + positionL, platform.y + 1, SPIKY_BUSH);
                }
                else if (percentageL < 0.85f)
                {
                    AddElementToCell(platform.startX + positionL, platform.y + 1, WOODEN_SPIKES);
                }


                if (percentageR < 0.25f)
                {
                    AddElementToCell(platform.startX + platform.width - positionR, platform.y + 1, WOLF);
                }
                else if (percentageR < 0.35f)
                {
                    AddElementToCell(platform.startX + platform.width - positionR, platform.y + 1, FROG);
                }
                else if (percentageR < 0.50f)
                {
                    AddElementToCell(platform.startX + platform.width - positionR, platform.y + 1, SPIKY_BUSH);
                }
                else if (percentageR < 0.85f)
                {
                    AddElementToCell(platform.startX + platform.width - positionR, platform.y + 1, WOODEN_SPIKES);
                }


                positionL = Random.Range(0, 2);

                for (int i = platform.startX + positionL; i < platform.startX + platform.width; i = i + 2)
                {
                    if (IsCellEmpty(i, platform.y + 1) ||
                        (IsElementInCell(i, platform.y + 1, TALL_PLATFORM_BODY) &&
                        IsElementInCell(i, platform.y + 1, EMPTY)))
                    {
                        AddElementToCell(i, platform.y + 1, GEM);
                    }
                }
            }
        }

        foreach (Vector2 mushroomPosition in mushrooms)
        {
            int x = (int)mushroomPosition.x;
            int gems = 0;

            for(int y = (int)mushroomPosition.y + 2;
                y < gridHeight && gems < 3 && (IsCellEmpty(x, y) ||
                (IsElementInCell(x, y, TALL_PLATFORM_BODY) && IsElementInCell(x, y, EMPTY)));
                y++)
            {
                AddElementToCell(x, y, GEM);
                gems++;
            }
        }

    }
    public void InstantiateGrid()
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

                    bool isLeftEdge = (x == 0) || (!IsElementInCell(x - 1, y, TALL_PLATFORM) && !IsElementInCell(x - 1, y, TALL_PLATFORM_BODY));
                    bool isRightEdge = (x == gridWidth - 1) || (!IsElementInCell(x + 1, y, TALL_PLATFORM) && !IsElementInCell(x + 1, y, TALL_PLATFORM_BODY));

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
                if (IsElementInCell(x, y, TALL_PLATFORM_BODY))
                {
                    bool isLeftEdge = (x == 0) || (!IsElementInCell(x - 1, y, TALL_PLATFORM) && !IsElementInCell(x - 1, y, TALL_PLATFORM_BODY));
                    bool isLeftCorner = false;
                    bool isRightEdge = (x == gridWidth - 1) || (!IsElementInCell(x + 1, y, TALL_PLATFORM) && !IsElementInCell(x + 1, y, TALL_PLATFORM_BODY));
                    bool isRightCorner = false;
                    if ((x > 0) && (x < gridWidth - 1))
                    {
                        isLeftCorner = IsElementInCell(x - 1, y, TALL_PLATFORM) && IsElementInCell(x + 1, y, TALL_PLATFORM_BODY);
                        isRightCorner = IsElementInCell(x + 1, y, TALL_PLATFORM) && IsElementInCell(x - 1, y, TALL_PLATFORM_BODY);
                    }

                    if (isLeftCorner)
                    {
                        Instantiate(Resources.Load("Prefabs/TallPlatform7"), new Vector3(x, y, 0), Quaternion.identity);
                    }
                    else if (isRightCorner)
                    {
                        Instantiate(Resources.Load("Prefabs/TallPlatform8"), new Vector3(x, y, 0), Quaternion.identity);
                    }
                    else if (isLeftEdge)
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
                if (IsElementInCell(x, y, MUSHROOM))
                {
                    Instantiate(Resources.Load("Prefabs/BouncyShroom"), new Vector3(x, y, 0), Quaternion.identity);
                }
                if (IsElementInCell(x, y, SPIKY_BUSH))
                {
                    Instantiate(Resources.Load("Prefabs/SpikyBush"), new Vector3(x, y, 0), Quaternion.identity);
                }
                if (IsElementInCell(x, y, WOODEN_SPIKES))
                {
                    Instantiate(Resources.Load("Prefabs/WoodenSpikes"), new Vector3(x, y, 0), Quaternion.identity);
                }
                if (IsElementInCell(x, y, FROG))
                {
                    Instantiate(Resources.Load("Prefabs/ToxicFrog"), new Vector3(x, y, 0), Quaternion.Euler(0, 180, 0));
                }
                if (IsElementInCell(x, y, WOLF))
                {
                    Instantiate(Resources.Load("Prefabs/SkullWolf"), new Vector3(x, y, 0), Quaternion.identity);
                }
                if (IsElementInCell(x, y, GEM))
                {
                    Instantiate(Resources.Load("Prefabs/BlueGem"), new Vector3(x, y, 0), Quaternion.identity);
                }
                if (IsElementInCell(x, y, PORTAL))
                {
                    Instantiate(Resources.Load("Prefabs/Portal"), new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }

        Debug.Log("Grid instantiated.");
    }

    string SerializeGrid()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                builder.Append(grid[x, y][0] + ",");
                builder.Append(grid[x, y][1] + ";"); // Save both slots
            }
            builder.AppendLine(); // Newline for readability
        }

        return builder.ToString();
    }

    void DeserializeGrid(string serializedGrid)
    {
        InitializeGrid();

        string[] rows = serializedGrid.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        for (int y = 0; y < gridHeight; y++)
        {
            string[] cells = rows[y].Split(';');
            for (int x = 0; x < gridWidth; x++)
            {

                string[] slots = cells[x].Split(',');
                grid[x, y][0] = int.Parse(slots[0]); // Primary slot
                grid[x, y][1] = int.Parse(slots[1]); // Secondary slot
            }
        }
    }

    void SaveGridState()
    {
        string serializedGrid = SerializeGrid();
        PlayerPrefs.SetString("SavedGrid", serializedGrid);
        PlayerPrefs.Save(); // Ensure it persists
    }

    void LoadGridState()
    {
        if (PlayerPrefs.HasKey("SavedGrid"))
        {
            string serializedGrid = PlayerPrefs.GetString("SavedGrid");
            DeserializeGrid(serializedGrid);
            InstantiateGrid(); // Reinstantiate the saved level
        }
        else
        {
            Debug.LogError("No saved grid state found!");
        }
    }

    void SavePlayerState(Vector2 position)
    {
        PlayerPrefs.SetFloat("PlayerX", position.x);
        PlayerPrefs.SetFloat("PlayerY", position.y);
        PlayerPrefs.Save();
    }

    Vector2 LoadPlayerState()
    {
        float x = PlayerPrefs.GetFloat("PlayerX", witchSpawnPosition.x); // Default to spawn
        float y = PlayerPrefs.GetFloat("PlayerY", witchSpawnPosition.y);
        return new Vector2(x, y);
    }


}
