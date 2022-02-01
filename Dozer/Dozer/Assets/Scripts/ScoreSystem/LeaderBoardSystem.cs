using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

public class LeaderBoardSystem : LeaderboardsAbstract
{
    public List<string> names;

    protected override Action<string, int> ScoreChanged
    {
        get => ActionSys.ScoreChanged;
        set => ActionSys.ScoreChanged = value;
    }
    
    public override List<string> GetLeaderBoard()
    {
        var leaderBoard = PlayerAndScoreDictionary 
            .OrderByDescending(pair => pair.Value)//Ordering
            .ToDictionary(pair => pair.Key, pair => pair.Value)//converting to dictionary to get keys
            .Keys; //get keys

        var leaderBoardAsAList = new List<string>(leaderBoard);
        return leaderBoardAsAList;
    }
    
    public override List<string> GetLeaderBoard(int playerCount)
    {
        var leaderBoard = PlayerAndScoreDictionary 
            .OrderByDescending(pair => pair.Value)//Ordering
            .Take(playerCount)//Taking head of list
            .ToDictionary(pair => pair.Key, pair => pair.Value)//converting to dictionary to get keys
            .Keys; //get keys

        var leaderBoardAsAList = new List<string>(leaderBoard);
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

    private static string RandomString(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
