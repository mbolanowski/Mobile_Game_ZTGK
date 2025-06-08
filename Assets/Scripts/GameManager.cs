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
            ScoreText.text = ScoreCounter.Instance.GetScore().ToString();
        }
    }

    public void TriggerGameOver()
    {
        gameRunning = false;
        playerMovement.isDead = true;
        HealthBar.SetActive(false);
        DeathScreen.SetActive(true);
        Score.SetActive(false);
        FinalScore.text = "Final Score: " + ScoreText.text;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
