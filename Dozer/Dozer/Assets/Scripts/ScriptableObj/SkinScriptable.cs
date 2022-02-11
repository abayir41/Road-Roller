using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Skin", menuName = "SkinMenu")]
public class SkinScriptable : ScriptableObject , IPurchasableDozer
{
    public string ItemID => itemID;
    [SerializeField] private string itemID;

    public int Price => price;
    [SerializeField] public int price;

    public GameObject DozerSkin => dozerSkin;
    [SerializeField] private GameObject dozerSkin;

    public Image Preview => preview;
    [SerializeField] private Image preview;

}
