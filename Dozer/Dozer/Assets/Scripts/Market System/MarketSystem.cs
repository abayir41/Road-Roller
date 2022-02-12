using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MarketSystem : MonoBehaviour
{
    public static MarketSystem Instance;

    public const string PurchasedString = "Purchased";
    public const string MoneyString = "Money";
    public const string SelectedSkin = "SelectedSkin";
    
    public List<SkinScriptable> DozerSkins => dozerSkins;
    [SerializeField] private List<SkinScriptable> dozerSkins;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private int Money => RegisterSystem.Instance.GetDataAsInt(MoneyString);

    public bool IsItemPurchased(string item)
    {
        return RegisterSystem.Instance.GetDataAsString(item).Equals(PurchasedString);
    }

    public void StateItemAsPurchased(string item)
    {
        RegisterSystem.Instance.SaveData(item,PurchasedString);
    }

    public bool CanBePurchased(string item)
    {
        return !RegisterSystem.Instance.GetDataAsString(item).Equals(PurchasedString) && Money >= GetItemPrice(item);
    }

    public void AddMoney(int amount)
    {
        RegisterSystem.Instance.SaveData(MoneyString,Money+amount);
    }

    public void RemoveMoney(int amount)
    {
        RegisterSystem.Instance.SaveData(MoneyString,Money-amount);
    }

    public int GetItemPrice(string item)
    {
        return RegisterSystem.Instance.GetDataAsInt(item);
    }

    public void SetItemPrice(string item, int amount)
    {
        RegisterSystem.Instance.SaveData(item, amount);
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
