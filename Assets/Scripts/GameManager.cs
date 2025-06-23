using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            ScoreText.text = TurningScript.CurrentScore.ToString() + " x" + TurningScript.currentScoreMultiplier;
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
        FinalScore.text = "Final Score: " + TurningScript.CurrentScore.ToString();
        if(Leaderboard.Instance != null)
        {
            if(Leaderboard.Instance.currentUserScore < TurningScript.CurrentScore) Leaderboard.Instance.currentUserScore = TurningScript.CurrentScore;
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
        Debug.Log(Leaderboard.Instance);
        Leaderboard.Instance?.SaveCurrentScore();
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
}
