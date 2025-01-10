using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWinScreen : MonoBehaviour
{
    public TMP_Text scoreText;

    private MapGenerator mapGenerator;

    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void SetUp(int score, bool highscore)
    {
        gameObject.SetActive(true);
        if (highscore)
        {
            scoreText.text = "NEW HIGHSCORE!\n" + "GEMS FOUND: " + score.ToString();
        }
        else scoreText.text = "GEMS FOUND: " + score.ToString();

        mapGenerator = FindFirstObjectByType<MapGenerator>();
        if (mapGenerator != null)
        {
            scoreText.text = "GEMS FOUND: " + score.ToString() + "\nTOTAL GEMS: " + mapGenerator.getGemCount();
        }
    }

    public void RestartButton()
    {
        audioManager.PlaySFX(audioManager.button);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MenuButton()
    {
        audioManager.PlaySFX(audioManager.button);
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("TitleScreen");
    }

    public void NextLevelButton()
    {
        audioManager.PlaySFX(audioManager.button);
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel + 1);
    }

    public void RestartGeneratedLevelButton()
    {
        audioManager.PlaySFX(audioManager.button);
        GameObject mapGeneratorGO = GameObject.Find("Map Generator");
        MapGenerator mapGenerator = mapGeneratorGO.GetComponent<MapGenerator>();
        mapGenerator.RestartLevel();
    }

    public void NewGeneratedLevel()
    {
        audioManager.PlaySFX(audioManager.button);
        PlayerPrefs.DeleteAll();
        GameController.gameController.ClearCurrentGeneratedLevelData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SaveMenuButton()
    {
        audioManager.PlaySFX(audioManager.button);
        SaveMenu saveMenu = FindFirstObjectByType<SaveMenu>();

        saveMenu.SetSourceMenu(SaveMenu.SaveMenuSource.WinScreen);
        saveMenu.gameObject.SetActive(true);
        saveMenu.SetSaveMenu(true);

        gameObject.SetActive(false);
    }
}
