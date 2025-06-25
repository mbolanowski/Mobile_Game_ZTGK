using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Windows.Speech;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TurningScript playerMovement;

    public GameObject HealthBar;
    public GameObject DeathScreen;
    public GameObject Score;
    public GameObject Multi;
    public GameObject SpeedIndicators;

    public TextMeshProUGUI FinalScore;

    private TextMeshProUGUI ScoreText;

    private TextMeshProUGUI MultiText;

    public TextMeshProUGUI speedText;

    private bool gameRunning = true;

    public Animator animator;

    public GameObject LeaderBoard;

    public TextMeshProUGUI CurrentPlacement;
    public TextMeshProUGUI HighScore;

    public GameObject ScoreSaving;
    public TMP_InputField field;

    [Header("Speed Sprites")] 
    public RawImage bird;
    public RawImage[] leftImages;
    public RawImage[] rightImages;

    private float lastMultiplier = 1.0f;
    private Tween multiTextTween;

    void Start()
    {
        Instance = this;
        ScoreText = Score.GetComponentInChildren<TextMeshProUGUI>();
        ScoreText.text = "0";

        MultiText = Multi.GetComponentInChildren<TextMeshProUGUI>();
        MultiText.text = "";
    }

    public void FixedUpdate()
    {
        if (gameRunning)
        {
            //ScoreText.text = playerMovement.CurrentScore.ToString() + " x" + playerMovement.currentScoreMultiplier;
            ScoreText.text = playerMovement.CurrentScore.ToString();
            float multiplier = playerMovement.currentScoreMultiplier;

            if (playerMovement.CurrentSpeedLevel != 4)
            {
                speedText.text = "speed " + (playerMovement.CurrentSpeedLevel + 1);
            }
            else
            {
                speedText.text = "speed MAX";
            }
            

            if (multiplier > 1.0f)
            {
                Multi.SetActive(true);

                // Only animate if multiplier changed
                if (Mathf.Abs(multiplier - lastMultiplier) > Mathf.Epsilon)
                {
                    MultiText.text = multiplier.ToString("F0") + "x";

                    // Animate scale
                    if (multiTextTween != null && multiTextTween.IsActive())
                        multiTextTween.Kill();

                    MultiText.rectTransform.localScale = Vector3.one * 3f; // reset
                    multiTextTween = MultiText.rectTransform.DOPunchScale(Vector3.one * 0.5f, 0.5f, 1, 0.5f);

                    // Change color to redder with higher multiplier (cap at 5x)
                    float t = Mathf.InverseLerp(1f, 5f, multiplier); // 1x = white, 5x = red
                    MultiText.color = Color.Lerp(Color.white, Color.red, t);

                    lastMultiplier = multiplier;
                }
            }
            else
            {
                Multi.SetActive(false);
                lastMultiplier = 1.0f;
            }
        }
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

    public void TriggerGameOver()
    {
        gameRunning = false;
        animator.enabled = false;
        playerMovement.isDead = true;
        HealthBar.SetActive(false);
        DeathScreen.SetActive(true);
        Score.SetActive(false);
        Multi.SetActive(false);
        SpeedIndicators.SetActive(false);
        speedText.text = "";
        hideAllSprites();
        FinalScore.text = "Final Score: " + playerMovement.CurrentScore.ToString();
        if(Leaderboard.Instance != null)
        {
            if(Leaderboard.Instance.currentUserScore < playerMovement.CurrentScore) Leaderboard.Instance.currentUserScore = playerMovement.CurrentScore;
            CurrentPlacement.text = Leaderboard.Instance.GetCurrentScorePlace().ToString();
            HighScore.text = "High Score: " + Leaderboard.Instance.currentUserScore.ToString();
        }
        
    }

    private void hideAllSprites()
    {
        leftImages[0].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        leftImages[1].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        leftImages[2].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        rightImages[0].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        rightImages[1].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        rightImages[2].rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        bird.rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
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

