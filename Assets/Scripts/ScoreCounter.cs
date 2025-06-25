using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance;

    public float TimeToScoreRatio = 1.0f; // Points per second

    private float elapsedTime = 0f;

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public int GetScore()
    {
        return Mathf.FloorToInt(elapsedTime * TimeToScoreRatio);
    }
}
