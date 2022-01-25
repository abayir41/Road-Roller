
using System;
using System.Collections.Generic;
using System.Linq;
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

    public abstract List<string> GetLeaderBoard(int playerCount);
}

