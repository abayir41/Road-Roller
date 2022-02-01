using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MarketSystem : MonoBehaviour
{
    public static readonly string PurchasedString = "Purchased";
    public static readonly string MoneyString = "Money";
    public static readonly string SelectedSkin = "SelectedSkin";
    
    
    private IRegisterSystem _registerSystem => GameController.Instance.RegisterSystem;
    public List<SkinScriptable> DozerSkins => dozerSkins;
    [SerializeField] private List<SkinScriptable> dozerSkins;

    private int Money => _registerSystem.GetDataAsInt(MoneyString);

    public bool IsItemPurchased(string item)
    {
        return _registerSystem.GetDataAsString(item).Equals(PurchasedString);
    }

    public void StateItemAsPurchased(string item)
    {
        _registerSystem.SaveData(item,PurchasedString);
    }

    public bool CanBePurchased(string item)
    {
        return !_registerSystem.GetDataAsString(item).Equals(PurchasedString) && Money >= GetItemPrice(item);
    }

    public void AddMoney(int amount)
    {
        _registerSystem.SaveData(MoneyString,Money+amount);
    }

    public void RemoveMoney(int amount)
    {
        _registerSystem.SaveData(MoneyString,Money-amount);
    }

    public int GetItemPrice(string item)
    {
        return _registerSystem.GetDataAsInt(item);
    }

    public void SetItemPrice(string item, int amount)
    {
        _registerSystem.SaveData(item, amount);
    }
    
    public void Purchase(string item)
    {
        if (!CanBePurchased(item))
        { 
            return; //to do handle this situation
        }
        var index = dozerSkins.IndexOf(dozerSkins.First(scriptable => scriptable.ItemID == item));
        RemoveMoney(GetItemPrice(item));
        StateItemAsPurchased(item);
        ActionSys.SkinPurchased?.Invoke(dozerSkins[index]);
    }
    
}
