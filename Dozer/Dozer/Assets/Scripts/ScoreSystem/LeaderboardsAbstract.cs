﻿
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class LeaderboardsAbstract : MonoBehaviour
{
    protected Dictionary<string, Player> PlayerAndScoreDictionary;

    public static LeaderBoardSystem Instance;  

    protected virtual void Awake()
    {
        PlayerAndScoreDictionary = new Dictionary<string, Player>();
    }
    
    public int TotalPlayerCount => PlayerAndScoreDictionary.Count;

    public int AlivePlayerCount => PlayerAndScoreDictionary.Count(pair => !pair.Value.IsDead);

    public void AddPlayer(string playerName, Player player)
    {
        PlayerAndScoreDictionary.Add(playerName, player);    
    }
    
    public void RemovePlayer(string playerName)
    {
        PlayerAndScoreDictionary.Remove(playerName);
    }

    public int GetPlayerRank(string player, bool addDeadPlayers = false)
    {
        return GetLeaderBoard(addDeadPlayers).IndexOf(player) + 1;
    }

    public void GetLeaderBoardString()
    {
        foreach (var player in GetLeaderBoard())
        {
            Debug.Log(player);
        }
    }

    public abstract List<string> GetLeaderBoard(bool addDeadPlayers = false);
    public abstract List<string> GetLeaderBoard(int playerCount, bool addDeadPlayers = false);
    
}

