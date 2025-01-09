using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{

    public TMP_Text scoreText;

    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void SetUp(int score)
    {
        gameObject.SetActive(true);
        scoreText.text = score.ToString() + " POINTS";
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

    public void RestartGeneratedLevelButton()
    {
        audioManager.PlaySFX(audioManager.button);

        GameObject mapGeneratorGO = GameObject.Find("Map Generator");
        MapGenerator mapGenerator = mapGeneratorGO.GetComponent<MapGenerator>();
        mapGenerator.RestartLevel(); 
    }

    public void SaveMenuButton()
    {
        audioManager.PlaySFX(audioManager.button);

        SaveMenu saveMenu = FindFirstObjectByType<SaveMenu>();

        saveMenu.SetSourceMenu(SaveMenu.SaveMenuSource.LoseScreen);
        saveMenu.gameObject.SetActive(true);
        saveMenu.SetSaveMenu(true);

        gameObject.SetActive(false);
    }
}
