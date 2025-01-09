using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gameController;

    // Regular level highscores
    public int[] regularLevelHighscores = new int[2];

    // Save slots for generated levels (4 slots, each with a seed and highscore)
    public SeedSlot[] saveSlots = new SeedSlot[4];

    public string currentGeneratedLevelSeed = "";
    public int currentGeneratedLevelHighscore = 0;

    public bool isRestart = false;

    void Awake()
    {
        //To make sure there is only one GameController SINGLETON DESIGN
        if (gameController == null)
        {
            DontDestroyOnLoad(gameObject);
            gameController = this;
            Load();
        }
        else if (gameController != this)
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter(); //We make it so the user won't be able to understand the data if they find the file
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat"); //Create file in unity persistent data path 

        PlayerData data = new PlayerData
        {
            regularHighscores = regularLevelHighscores,
            saveSlots = saveSlots
        };

        bf.Serialize(file, data); //writes data to the file
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            // Load data
            regularLevelHighscores = data.regularHighscores;
            saveSlots = data.saveSlots;
        }
        else
        {
            // Initialize empty save slots
            for (int i = 0; i < saveSlots.Length; i++)
            {
                saveSlots[i] = new SeedSlot();
            }
        }
    }

    public void StartGeneratedLevel(string seed)
    {
 
        if(!isRestart)
        {
            currentGeneratedLevelSeed = seed;
            currentGeneratedLevelHighscore = GetGeneratedLevelHighscore(seed);
        }   

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.UpdateHighscore(currentGeneratedLevelHighscore);
        }

        isRestart = false;
    }


    public void LoadGeneratedLevel(bool restart)
    {   
        isRestart = restart;
        SceneManager.LoadScene("AutoLevel");
    }

    public void ClearCurrentGeneratedLevelData()
    {
        currentGeneratedLevelSeed = "";
        currentGeneratedLevelHighscore = 0;
    }

    public void UpdateRegularLevelHighscore(int levelIndex, int newScore)
    {

        if (levelIndex >= 0 && levelIndex < regularLevelHighscores.Length 
            && newScore > regularLevelHighscores[levelIndex])
        {
            regularLevelHighscores[levelIndex] = newScore;
            Save();
        }
    }

    public void UpdateGeneratedLevelHighscore(string seed, int newScore)
    {
        int levelIndex = GetIndexFromSeed(seed);
        if (levelIndex >= 0 && levelIndex < saveSlots.Length
            && newScore > saveSlots[levelIndex].highscore)
        {
            saveSlots[levelIndex].highscore = newScore;
            Save();
        }
    }

    public int GetIndexFromSeed(string seed)
    {
        for(int i = 0; i < saveSlots.Length; i++)
        {
            if (saveSlots[i].seed == seed)
            {
                return i;
            }
        }

        return -1;
    }

    public void SaveCurrentGeneratedLevel(int slotIndex, string name)
    {

        if (slotIndex >= 0 && slotIndex < saveSlots.Length && currentGeneratedLevelSeed != "")
        {
            saveSlots[slotIndex] = new SeedSlot
            {
                seed = currentGeneratedLevelSeed,
                highscore = currentGeneratedLevelHighscore,
                name = name
            };
            Save();

            Debug.Log("Slot saved! highscore = " + saveSlots[slotIndex].highscore);
        }
    }
    
    public SeedSlot GetSeedSlot(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < saveSlots.Length)
        {
            return saveSlots[slotIndex];
        }

        else return new SeedSlot();

    }

    public string GetSeedSlotName(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < saveSlots.Length)
        {
            if (saveSlots[slotIndex].name == null || saveSlots[slotIndex].name == "") return "EMPTY SLOT";
            else return saveSlots[slotIndex].name;
        }

        else return "INVALID INDEX";
    }

    public int GetGeneratedLevelHighscore(string seed)
    {
        foreach (SeedSlot slot in saveSlots)
        {
            if (slot.seed == seed)
            {
                return slot.highscore;
            }
        }

        return 0; // Default highscore for unsaved generated levels
    }
}

[Serializable]
public class SeedSlot
{
    public string seed = ""; // The seed for the generated level
    public int highscore = 0; // The highscore for the level
    public string name = ""; // The name for the save slot
}

[Serializable]
class PlayerData
{
    public int[] regularHighscores;
    public SeedSlot[] saveSlots;
}
