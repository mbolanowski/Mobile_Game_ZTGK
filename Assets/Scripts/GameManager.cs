using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TurningScript playerMovement;

    public GameObject HealthBar;
    public GameObject DeathScreen;
    public GameObject Score;

    public TextMeshProUGUI FinalScore;

    private TextMeshProUGUI ScoreText;

    private bool gameRunning = true;

    public Animator animator;

    public GameObject LeaderBoard;

    public TextMeshProUGUI CurrentPlacement;
    public TextMeshProUGUI HighScore;

    public GameObject ScoreSaving;
    public TMP_InputField field;

    void Start()
    {
        Instance = this;
        ScoreText = Score.GetComponentInChildren<TextMeshProUGUI>();
        ScoreText.text = "0";
    }

    public void FixedUpdate()
    {
        if (gameRunning)
        {
            ScoreText.text = playerMovement.CurrentScore.ToString() + " x" + TurningScript.currentScoreMultiplier;
        }
    }

    public void TriggerGameOver()
    {
        gameRunning = false;
        animator.enabled = false;
        playerMovement.isDead = true;
        HealthBar.SetActive(false);
        DeathScreen.SetActive(true);
        Score.SetActive(false);
        FinalScore.text = "Final Score: " + playerMovement.CurrentScore.ToString();
        if(Leaderboard.Instance != null)
        {
            if(Leaderboard.Instance.currentUserScore < playerMovement.CurrentScore) Leaderboard.Instance.currentUserScore = playerMovement.CurrentScore;
            CurrentPlacement.text = Leaderboard.Instance.GetCurrentScorePlace().ToString();
            HighScore.text = "High Score: " + Leaderboard.Instance.currentUserScore.ToString();
        }
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        SceneManager.LoadScene(1);
    }

    public void SaveScore()
    {
        Debug.Log(field.text);
        Leaderboard.Instance?.SaveCurrentScore(field.text);
        HideSaveScore();
    }

    public void ShowLeaderboard()
    {
        LeaderBoard.SetActive(true);
        DeathScreen.SetActive(false);
    }

    public void HideLeaderboard()
    {
        LeaderBoard.SetActive(false);
        DeathScreen.SetActive(true);
    }

    public void ShowSaveScore()
    {
        ScoreSaving.SetActive(true);
        DeathScreen.SetActive(false);
        field.Select();
    }

    public void HideSaveScore()
    {
        ScoreSaving.SetActive(false);
        DeathScreen.SetActive(true);
        field.text = "";
    }
}

