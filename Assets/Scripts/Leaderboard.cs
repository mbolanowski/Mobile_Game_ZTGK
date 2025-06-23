using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerEntry
{
    public string Name;
    public int Score;
}

public struct ListContainer
{
    public List<PlayerEntry> dataList;

    public ListContainer(List<PlayerEntry> _dataList)
    {
        dataList = _dataList;
    }
}


public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance;
    public List<PlayerEntry> playerScores;

    public string filename = "leaderboard.txt";
    public int currentUserScore = 0;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        try
        {
            using (var sr = new StreamReader(Application.dataPath + "/" + filename))
            {
                string leaderBoardText = sr.ReadToEnd();
                sr.Close();
                ListContainer container = JsonUtility.FromJson<ListContainer>(leaderBoardText);
                playerScores = container.dataList;
            }
        }
        catch
        {
            playerScores = new List<PlayerEntry>();
        }
    }

    private void OnDestroy()
    {
        SerializeScoreboard();
    }

    public void SaveCurrentScore(string username = "")
    {
        PlayerEntry newEntry = new PlayerEntry();
        newEntry.Score = currentUserScore;
        if(username.Length == 0)
        {
            username = "PLAYER_NO_" + playerScores.Count;
        }
        newEntry.Name = username;
        playerScores.Add(newEntry);
        SortByScore();
    }

    public void SortByScore()
    {
        playerScores.Sort((x,y) => y.Score.CompareTo(x.Score));
    }

    public void SerializeScoreboard()
    {
        ListContainer container = new ListContainer(playerScores);
        string json = JsonUtility.ToJson(container);
        var sw = new StreamWriter(Application.dataPath + "/" + filename);
        sw.Write(json);
        sw.Close();
    }

    public int GetCurrentScorePlace()
    {
        for(int i = 0; i < playerScores.Count; i++)
        {
            if (playerScores[i].Score < currentUserScore) return i + 1;
        }
        return playerScores.Count + 1;
    }


}
