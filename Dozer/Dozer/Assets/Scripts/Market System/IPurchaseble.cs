using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


