using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pausePanel;

    private bool isSaveMenuOpen = false;

    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause") && !isSaveMenuOpen)
        {
            
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        audioManager.PlaySFX(audioManager.button);
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        audioManager.PlaySFX(audioManager.button);
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        isPaused = false;
    }

    public void MainMenu()
    {
        audioManager.PlaySFX(audioManager.button);
        PlayerPrefs.DeleteAll();
        Time.timeScale = 1;
        isPaused = false;
        SceneManager.LoadScene("TitleScreen");
    }

    public void SaveMenuButton()
    {
        audioManager.PlaySFX(audioManager.button);
        SaveMenu saveMenu = FindFirstObjectByType<SaveMenu>();

        saveMenu.SetSourceMenu(SaveMenu.SaveMenuSource.PauseMenu);
        saveMenu.gameObject.SetActive(true);
        saveMenu.SetSaveMenu(true);

        pausePanel.SetActive(false);
        isSaveMenuOpen = true;
    }

    public void CloseSaveMenu()
    {
        audioManager.PlaySFX(audioManager.button);
        pausePanel.SetActive(true);
        isSaveMenuOpen = false;
    }

    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
