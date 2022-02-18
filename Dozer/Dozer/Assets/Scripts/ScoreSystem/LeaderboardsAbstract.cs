using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class LeaderboardsAbstract : MonoBehaviour, ISystem
{
    protected List<Player> PlayerList;

    public static LeaderBoardSystem Instance { get; protected set; }  

    protected virtual void Awake()
    {
        PlayerList = new List<Player>();
    }
    
    public int TotalPlayerCount => PlayerList.Count;

    public int AlivePlayerCount => PlayerList.Count(pair => !pair.IsDead);

    public void AddPlayer(Player player)
    {
        PlayerList.Add(player);    
    }
    
    public void RemovePlayer(Player player)
    {
        PlayerList.Remove(player);
    }

    public Player GetPlayerByName(string playerName)
    {
        return PlayerList.First(player => player.Name.Equals(playerName));
    }
    
    public int GetPlayerRank(Player player, bool addDeadPlayers = false)
    {
        return GetLeaderBoard(addDeadPlayers).IndexOf(player) + 1;
    }

    public void ResetTheSystem()
    {
        PlayerList = new List<Player>();
    }
    
    public abstract List<Player> GetLeaderBoard(bool addDeadPlayers = false);
    public abstract List<Player> GetLeaderBoard(int playerCount, bool addDeadPlayers = false);
    
}

