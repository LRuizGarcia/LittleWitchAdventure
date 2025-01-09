using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject selectLevelMenu;
    public GameObject levelGeneratorMenu;
    public GameObject loadMenu;

    public GameObject controlsPanel;

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        GameController.gameController.ClearCurrentGeneratedLevelData();
    }

    public void SelectLevelButton()
    {
        gameObject.SetActive(false);
        selectLevelMenu.SetActive(true);
    }

    public void LevelGeneratorButton()
    {
        gameObject.SetActive(false);
        levelGeneratorMenu.SetActive(true);
    }

    public void LoadLevelButton()
    {
        gameObject.SetActive(false);
        loadMenu.SetActive(true);
    }

    public void BackButton()
    {
        if (loadMenu.activeSelf) levelGeneratorMenu.SetActive(true);
        else mainMenu.SetActive(true);

        gameObject.SetActive(false);
    }

    public void ControlsButton()
    {
        controlsPanel.SetActive(true);
    }

    public void CloseControlsPanel() 
    {
        controlsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Level1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Level2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void GenerateLevel()
    {
        SceneManager.LoadScene("AutoLevel");
    }
}
