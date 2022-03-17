using System.Collections.Generic;
using System.Linq;
using RandomNameGeneratorLibrary;
using UnityEngine;
using Random = System.Random;
using UnityRandom = UnityEngine.Random;

public class LeaderBoardSystem : LeaderboardsAbstract
{
    private List<string> names;
    private List<Color> colors => GameController.GameConfig.Colors;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        var pGen = new PersonNameGenerator();
        names = pGen.GenerateMultipleMaleFirstNames(15).ToList();
    }

    public override List<Player> GetLeaderBoard(bool addDeadPlayers = false)
    {
        var leaderBoard = PlayerList
            .Where(pair => !pair.IsDead || addDeadPlayers) //selecting according to dead or not 
            .OrderByDescending(pair => pair.Score) //Ordering
            .ToList();
        
        return leaderBoard;
    }
    
    public override List<Player> GetLeaderBoard(int playerCount, bool addDeadPlayers = false)
    {
        var leaderBoard = PlayerList
            .Where(pair => !pair.IsDead || addDeadPlayers) //selecting according to dead or not 
            .OrderByDescending(pair => pair.Score) //Ordering
            .Take(playerCount) //Taking head of list
            .ToList();

        
        return leaderBoard;
    }


    public string GetRandomName()
    {
        
        var suitableNames = names.Where(player => PlayerList.All(player1 => player1.Name != player)).ToArray();
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
        var suitableColors = colors.Where(player => PlayerList.All(pair => pair.PlayerColor != player)).ToArray();
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
        );
    }
}
