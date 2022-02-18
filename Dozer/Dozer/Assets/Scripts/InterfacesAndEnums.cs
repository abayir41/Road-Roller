using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#region Enums

public enum GameStatus
{
    Loading,
    WaitingOnMenu,
    Playing,
    Paused,
    Lost,
    Ended
}

public enum GameMode
{
    BeTheLast,
    TimeCounting
}

public enum ObjectType
{
    Small,
    Mid,
    Big,
    Mega
}

#endregion

#region ColoringInterfaces

public interface IColorChanger
{
    void ChangeColor(Color color, int materialIndex);
}

public interface IColorChangerRandomly : IColorChanger
{
    Color[] Colors { get; }
    void SelectColorRandomly(Color[] presetColors,int materialIndex);
    List<int> PaintMaterialIndexes { get;}
}

public interface IPurchasable
{
    string ItemID { get; }
    
    int ScoreThreshold { get; }
}

public interface IPurchasableDozer : IPurchasable
{
    GameObject DozerSkin { get; }
    
    Sprite Preview { get; }
}

#endregion

#region SystemInterfaces

public interface ISteerSystem
{ 
    float Angle { get; }
    
}

public interface IObjectScanner
{
    List<string> Filter { get;}
    List<GameObject> ScannedObjects { get; }
}

public interface IRegisterSystem
{
    void SaveData(string key, int value);
    void SaveData(string key, float value);
    void SaveData(string key, string value);

    int GetDataAsInt(string key);
    float GetDataAsFloat(string key);
    string GetDataAsString(string key);
}

public interface IInteractable
{
    ObjectType ObjectType { get; }
    
    int DestroyThreshold { get; }
    
    int ObjectHitPoint { get; }
    
    void Interact(PlayerController playerController);
    
    bool IsDozer { get; }
}

public interface ISystem
{
    void ResetTheSystem();
}



#endregion

#region MiniClasses
public class Player
{
    public string Name { get; }
    public int Score { get; set; }
    public int KillCount { get; set; }
    public Color PlayerColor { get; }
    public int Level { get; set; }
    public bool IsDead { get; set; }

    public Player(string name, int startScore, Color playerColor)
    {
        PlayerColor = playerColor;
        Name = name;
        Score = startScore;
        Level = 1;
        IsDead = false;
    }
}
#endregion

