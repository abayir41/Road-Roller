using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IPurchasable
{
    string ItemID { get; }
    int Price { get; }
}

public interface IPurchasableDozer : IPurchasable
{
    GameObject DozerSkin { get; }
    
    Image Preview { get; }
}


