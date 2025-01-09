using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject selectLevelMenu;
    public GameObject levelGeneratorMenu;
    public GameObject loadMenu;

    public GameObject controlsPanel;

    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        GameController.gameController.ClearCurrentGeneratedLevelData();
    }

    public void SelectLevelButton()
    {
        gameObject.SetActive(false);
        selectLevelMenu.SetActive(true);
        audioManager.PlaySFX(audioManager.button);
    }

    public void LevelGeneratorButton()
    {
        gameObject.SetActive(false);
        levelGeneratorMenu.SetActive(true);
        audioManager.PlaySFX(audioManager.button);
    }

    public void LoadLevelButton()
    {
        gameObject.SetActive(false);
        loadMenu.SetActive(true);
        audioManager.PlaySFX(audioManager.button);
    }

    public void BackButton()
    {
        if (loadMenu.activeSelf) levelGeneratorMenu.SetActive(true);
        else mainMenu.SetActive(true);

        gameObject.SetActive(false);
        audioManager.PlaySFX(audioManager.button);
    }

    public void ControlsButton()
    {
        controlsPanel.SetActive(true);
        audioManager.PlaySFX(audioManager.button);
    }

    public void CloseControlsPanel() 
    {
        controlsPanel.SetActive(false);
        audioManager.PlaySFX(audioManager.button);
    }

    public void QuitGame()
    {
        Application.Quit();
        audioManager.PlaySFX(audioManager.button);
    }

    public void Level1()
    {
        SceneManager.LoadScene("Level1");
        audioManager.PlaySFX(audioManager.button);
    }

    public void Level2()
    {
        SceneManager.LoadScene("Level2");
        audioManager.PlaySFX(audioManager.button);
    }

    public void GenerateLevel()
    {
        SceneManager.LoadScene("AutoLevel");
        audioManager.PlaySFX(audioManager.button);
    }
}
