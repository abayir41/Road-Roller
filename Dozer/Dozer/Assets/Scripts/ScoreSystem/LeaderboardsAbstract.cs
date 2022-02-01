
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class LeaderboardsAbstract : MonoBehaviour
{
    protected Dictionary<string, int> PlayerAndScoreDictionary;
    protected abstract Action<string, int> ScoreChanged { get; set;} 
    protected virtual void Awake()
    {
        PlayerAndScoreDictionary = new Dictionary<string, int>();
    }

    private void OnEnable()
    {
        ScoreChanged += AddScore;
    }

    private void OnDisable()
    {
        ScoreChanged -= AddScore;
    }

    public void SetScore(string player, int score)
    {
        if (PlayerAndScoreDictionary.ContainsKey(player))
        {
            PlayerAndScoreDictionary[player] = score;
        }
        else
        {
            PlayerAndScoreDictionary.Add(player,score);
        }
    }
    public void AddScore(string player, int score)
    {
        if (PlayerAndScoreDictionary.ContainsKey(player))
        {
            PlayerAndScoreDictionary[player] += score;
        }
        else
        {
            PlayerAndScoreDictionary.Add(player,score);
        }
    }

    public int PlayerCount => PlayerAndScoreDictionary.Count;

    public void RemovePlayer(string playerName)
    {
        PlayerAndScoreDictionary.Remove(playerName);
    }

    public int GetPlayerRank(string player)
    {
        return GetLeaderBoard().IndexOf(player) + 1;
    }
    
    public abstract List<string> GetLeaderBoard();
    public abstract List<string> GetLeaderBoard(int playerCount);
    
}

