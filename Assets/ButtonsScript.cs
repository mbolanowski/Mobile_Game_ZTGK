using UnityEngine;

public class ButtonsScript : MonoBehaviour
{
    public GameObject Leaderboard;
    public GameObject[] Others;

    public void ShowLeaderboard()
    {
        Leaderboard.SetActive(true);
        foreach (GameObject o in Others)
        {
            o.SetActive(false);
        }
    }

    public void HideLeaderboard()
    {
        Leaderboard.SetActive(false);
        foreach (GameObject o in Others)
        {
            o.SetActive(true);
        }
    }
}
