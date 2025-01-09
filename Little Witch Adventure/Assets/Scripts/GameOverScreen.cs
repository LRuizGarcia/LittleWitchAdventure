using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{

    public TMP_Text scoreText;

    public void SetUp(int score)
    {
        gameObject.SetActive(true);
        scoreText.text = score.ToString() + " POINTS";
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MenuButton()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("TitleScreen");
    }

    public void RestartGeneratedLevelButton()
    {
        GameObject mapGeneratorGO = GameObject.Find("Map Generator");
        MapGenerator mapGenerator = mapGeneratorGO.GetComponent<MapGenerator>();
        mapGenerator.RestartLevel(); 
    }

    public void SaveMenuButton()
    {
        SaveMenu saveMenu = FindFirstObjectByType<SaveMenu>();

        saveMenu.SetSourceMenu(SaveMenu.SaveMenuSource.LoseScreen);
        saveMenu.gameObject.SetActive(true);
        saveMenu.SetSaveMenu(true);

        gameObject.SetActive(false);
    }
}
