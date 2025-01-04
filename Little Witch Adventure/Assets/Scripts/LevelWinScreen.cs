using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWinScreen : MonoBehaviour
{
    public TMP_Text scoreText;

    public void SetUp(int score, bool highscore)
    {
        gameObject.SetActive(true);
        if (highscore)
        {
            scoreText.text = "NEW HIGHSCORE!\n" + score.ToString() + " POINTS";
        }
        else scoreText.text = score.ToString() + " POINTS";
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

    public void NextLevelButton()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel + 1);
    }

    public void RestartGeneratedLevelButton()
    {
        GameObject mapGeneratorGO = GameObject.Find("Map Generator");
        MapGenerator mapGenerator = mapGeneratorGO.GetComponent<MapGenerator>();
        mapGenerator.RestartLevel();
    }

    public void NewGeneratedLevel()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
