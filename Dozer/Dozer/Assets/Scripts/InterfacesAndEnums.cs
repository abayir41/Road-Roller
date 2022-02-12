using System.Collections.Generic;
using UnityEngine;

#region Enums

public enum GameStatus
{
    Loading,
    WaitingOnMenu,
    Playing,
    Paused,
    Ended
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
}

interface IRandomlyPaintedMaterialIndex
{
    List<int> MaterialIndexes { get;}
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

#endregion
