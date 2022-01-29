using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketSystem : MonoBehaviour
{
    private const string PurchasedString = "Purchased";
    private const string MoneyString = "Money";

    
    [SerializeField] private List<string> dozerSkinsStrings;
    [SerializeField] private List<int> dozerSkinsPrices;
    [SerializeField] private List<GameObject> dozerSkinGameObjects;
    [SerializeField] private List<Image> dozerSkinImages;
    
    [SerializeField] private RegisterSystem registerSystem;
    
    private IRegisterSystem _registerSystem;
    private List<Purchasable> _dozerSkins;

    private void Awake()
    {
        _registerSystem = registerSystem;
        _dozerSkins = new List<Purchasable>();
        
        for (var i = 0; i < dozerSkinsStrings.Count; i++)
        {
            var skin = new Purchasable(dozerSkinsStrings[i],dozerSkinsPrices[i],dozerSkinGameObjects[i],dozerSkinImages[i]);
            _dozerSkins.Add(skin);
        }
    }

    private int Money => registerSystem.GetDataAsInt(MoneyString);

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
        var index = dozerSkinsStrings.IndexOf(item);
        RemoveMoney(GetItemPrice(item));
        StateItemAsPurchased(item);
        ActionSys.ObjectPurchased?.Invoke(_dozerSkins[index]);
    }
    
}
