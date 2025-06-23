using TMPro;
using Unity.VisualScripting;
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
    
    [Header("Multiplier Sprites")]
    public RawImage[] leftImages;
    public RawImage[] rightImages;

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
            ScoreText.text = playerMovement.CurrentScore.ToString() + " x" + playerMovement.currentScoreMultiplier;
            switch (playerMovement.currentScoreMultiplier)
            {
                case 1.0f:
                {
                    leftImages[0].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    leftImages[1].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    leftImages[2].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    rightImages[0].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    rightImages[1].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    rightImages[2].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    break;
                }
                case 2.0f:
                {
                    float scale = (playerMovement.SpeedDecreaseTime - playerMovement.currentTime) / playerMovement.SpeedDecreaseTime;
                    leftImages[0].rectTransform.localScale = new Vector3(scale, scale, scale);
                    leftImages[1].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    leftImages[2].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    rightImages[0].rectTransform.localScale = new Vector3(scale, scale, scale);
                    rightImages[1].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    rightImages[2].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    break;
                }
                case 3.0f:
                {
                    float scale = (playerMovement.SpeedDecreaseTime - playerMovement.currentTime) / playerMovement.SpeedDecreaseTime;
                    leftImages[0].rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    leftImages[1].rectTransform.localScale = new Vector3(scale, scale, scale);
                    leftImages[2].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    rightImages[0].rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    rightImages[1].rectTransform.localScale = new Vector3(scale, scale, scale);
                    rightImages[2].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    break;
                }
                case 4.0f:
                {
                    float scale = (playerMovement.SpeedDecreaseTime - playerMovement.currentTime) / playerMovement.SpeedDecreaseTime;
                    leftImages[0].rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    leftImages[1].rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    leftImages[2].rectTransform.localScale = new Vector3(scale, scale, scale);
                    rightImages[0].rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    rightImages[1].rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    rightImages[2].rectTransform.localScale = new Vector3(scale, scale, scale);
                    break;
                }
                default:
                    break;
                
            }
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
