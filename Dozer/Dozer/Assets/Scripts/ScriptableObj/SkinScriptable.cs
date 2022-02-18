﻿using UnityEngine;

[CreateAssetMenu(fileName = "Skin", menuName = "SkinMenu")]
public class SkinScriptable : ScriptableObject , IPurchasableDozer
{
    public string ItemID => itemID;
    [SerializeField] private string itemID;

    public int ScoreThreshold => scoreThreshold;
    [SerializeField] public int scoreThreshold;

    public GameObject DozerSkin => dozerSkin;
    [SerializeField] private GameObject dozerSkin;

    public Sprite Preview => preview;
    [SerializeField] private Sprite preview;

}
