using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject otherMenu;

    private void Start()
    {
        PlayerPrefs.DeleteAll();
    }

    public void ChangeMenu()
    {
        this.gameObject.SetActive(false);
        otherMenu.SetActive(true);
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
