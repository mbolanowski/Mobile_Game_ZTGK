using UnityEngine;

public class LeaderboardVis : MonoBehaviour
{
    public GameObject CellPrefab;

    public GameObject Container;

    private void OnEnable()
    {
        if(Leaderboard.Instance == null) return;

        foreach(Transform child in Container.transform)
        {
            Destroy(child.gameObject);
        }

        int i = 1;
        foreach(PlayerEntry entry in Leaderboard.Instance.playerScores)
        {
            GameObject newCell = Instantiate(CellPrefab,Container.transform);
            Cell cell = newCell.GetComponent<Cell>();
            cell.Name.text = entry.Name;
            cell.Score.text = entry.Score.ToString();
            cell.Place.text = i.ToString();
            i++;
        }
    }
}
