using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionSys
{
    public static Action<IInteractable> ObjectGotHit;

    public static Action<int> LevelUpped;

    public static Action MaxLevelReached;

    public static Action<GameObject> ObjectDestroyed;
    
    public static Action<string, int> ScoreChanged;
}
