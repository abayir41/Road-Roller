using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionSys //These are globally called
{
    public static Action<IInteractable> ObjectGotHit;

    public static Action<int> LevelUpped;

    public static Action MaxLevelReached;

    public static Action<GameObject> ObjectDestroyed;
    
    public static Action<string, int> ScoreChanged;

    public static Action<IPurchasableDozer> SkinPurchased;

    public static Action<GameStatus> GameStatusChanged;
}

public class CarActionSys //These are dozer-based called
{
    public Action<IInteractable> ObjectGotHit;

    public Action<int> LevelUpped;

    public Action MaxLevelReached;
}

public enum GameStatus
{
    Playing,
    Paused,
    Ended
}