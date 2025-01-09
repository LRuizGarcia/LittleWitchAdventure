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

    private int currentLevel;
    private int score = 0;
    private int highscore = 0;
    private int regularLevels = 2;

    private bool isRegularLevel;

    private void Awake()
    {
        instance = this; //it creates a public instance of this object at the start of the game
    }

    void Start()
    {
        playerMovement.OnPlayerDeath += GameOver;
        playerMovement.OnLevelWin += LevelWin;


        currentLevel = SceneManager.GetActiveScene().buildIndex - 1;

        if (currentLevel >= 0 && currentLevel < regularLevels) isRegularLevel = true;
        else isRegularLevel = false;

        if (isRegularLevel) highscore = GameController.gameController.regularLevelHighscores[currentLevel];
        else highscore = GameController.gameController.currentGeneratedLevelHighscore;

        scoreText.text = "SCORE: " + score.ToString();
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();
    }

    void OnDestroy()
    {
        playerMovement.OnPlayerDeath -= GameOver;
        playerMovement.OnLevelWin += LevelWin;
    }

    public void UpdateHighscore(int newHighscore)
    {
        highscore = newHighscore;
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();
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
            if (isRegularLevel) GameController.gameController.UpdateRegularLevelHighscore(currentLevel, score);
            else
            {
                GameController.gameController.currentGeneratedLevelHighscore = score;
                GameController.gameController.UpdateGeneratedLevelHighscore(
                    GameController.gameController.currentGeneratedLevelSeed, score);
            }
            levelWinScreen.SetUp(score, true); //if it's a new highscore, it will show a message

        }
        else levelWinScreen.SetUp(score, false);

    }
}
