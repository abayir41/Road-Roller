using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using UnityRandom = UnityEngine.Random;

public class LeaderBoardSystem : LeaderboardsAbstract
{
    public List<string> names;
    public List<Color> colors;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    public override List<Player> GetLeaderBoard(bool addDeadPlayers = false)
    {
        var leaderBoard = PlayerAndScoreDictionary
            .Where(pair => !pair.Value.IsDead || addDeadPlayers) //selecting according to dead or not 
            .OrderByDescending(pair => pair.Value.Score)//Ordering
            .ToDictionary(pair => pair.Key, pair => pair.Value)//converting to dictionary to get keys
            .Values; //get keys

        var leaderBoardAsAList = new List<Player>(leaderBoard);
        return leaderBoardAsAList;
    }
    
    public override List<Player> GetLeaderBoard(int playerCount, bool addDeadPlayers = false)
    {
        var leaderBoard = PlayerAndScoreDictionary
            .Where(pair => !pair.Value.IsDead || addDeadPlayers) //selecting according to dead or not 
            .OrderByDescending(pair => pair.Value.Score)//Ordering
            .Take(playerCount)//Taking head of list
            .ToDictionary(pair => pair.Key, pair => pair.Value)//converting to dictionary to get keys
            .Values; //get keys

        var leaderBoardAsAList = new List<Player>(leaderBoard);
        return leaderBoardAsAList;
    }


    public string GetRandomName()
    {
        var suitableNames = names.Where(player => !PlayerAndScoreDictionary.ContainsKey(player)).ToArray();
        if (suitableNames.Length == 0)
        {
            return RandomString(5);
        }
        else
        {
            var random = new Random();
            return suitableNames[random.Next(suitableNames.Length)];
        }
    }

    public Color GetRandomColor()
    {
        var suitableColors = colors.Where(player => PlayerAndScoreDictionary.All(pair => pair.Value.PlayerColor != player)).ToArray();
        if (suitableColors.Length == 0)
        {
            return RandomColor();
        }
        else
        {
            var random = new Random();
            return suitableColors[random.Next(suitableColors.Length)];
        }
    }

    private static string RandomString(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private static Color RandomColor()
    {
        return new Color(
            UnityRandom.Range(0f, 1f), 
            UnityRandom.Range(0f, 1f), 
            UnityRandom.Range(0f, 1f)
        );;
    }
}
