using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal.Execution;
using UnityEngine;
using UnityEngine.UI;


public struct Purchasable : IPurchasable
{
    public string ItemID { get; }

    public int Price { get; }

    public GameObject DozerSkin { get; }

    public Image Preview { get; }

    public Purchasable(string itemID, int price, GameObject dozerSkin, Image preview)
    {
        ItemID = itemID;
        Price = price;
        DozerSkin = dozerSkin;
        Preview = preview;
    }   
    
}