using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance;

    private Vector3 startPosition;

    public float DistanceToScoreRatio;

    void Start()
    {
        Instance = this;
        startPosition = transform.position;
    }

    public int GetScore()
    {
        return (int)((transform.position - startPosition).magnitude * DistanceToScoreRatio);
    }

}
