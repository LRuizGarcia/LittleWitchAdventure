using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public PlayerMovement playerMovement;
    public GameOverScreen gameOverScreen;
    public LevelWinScreen levelWinScreen;

    public TMP_Text scoreText;
    public TMP_Text highscoreText;

    int currentLevel;
    int score = 0;
    int highscore = 0;

    private void Awake()
    {
        instance = this; //it creates a public instance of this object at the start of the game
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement.OnPlayerDeath += GameOver;
        playerMovement.OnLevelWin += LevelWin;


        currentLevel = SceneManager.GetActiveScene().buildIndex - 1;
        highscore = GameController.gameController.highscore[currentLevel];

        scoreText.text = "SCORE: " + score.ToString();
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();
    }

    void OnDestroy()
    {
        playerMovement.OnPlayerDeath -= GameOver;
        playerMovement.OnLevelWin += LevelWin;
    }

    public void AddPoint()
    {
        score += 1;
        scoreText.text = "SCORE: " + score.ToString();

    }

    public void GameOver()
    {
        gameOverScreen.SetUp(score);
    }

    public void LevelWin()
    {
        if (score > highscore)
        {
            GameController.gameController.highscore[currentLevel] = score;
            GameController.gameController.Save();
            levelWinScreen.SetUp(score, true); //if it's a new highscore, it will show a message

        }
        else levelWinScreen.SetUp(score, false);

    }
}
