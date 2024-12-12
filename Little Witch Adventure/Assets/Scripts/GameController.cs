using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gameController;

    public int[] highscore = new int [] { 0, 0, 0 }; //To store the highscore of each level


    void Awake()
    {
        //To make sure there is only one GameController SINGLETON DESIGN
        if(gameController == null)
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

        PlayerData data = new PlayerData();
        data.highscore = highscore;

        bf.Serialize(file, data); //writes data to the file
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            highscore = data.highscore;
        }

    }


}

[Serializable]
class PlayerData
{
    public int[] highscore;
}
