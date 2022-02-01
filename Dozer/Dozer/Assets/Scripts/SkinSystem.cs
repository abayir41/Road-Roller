using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class SkinSystem : MonoBehaviour
{
    private void Start()
    {
        var visualPoint = GetComponent<CarSystem>().VisualPoint;
        var isAI = GetComponent<PlayerController>().IsAI;
        if (isAI)
        {
            var random = new Random();
            var randomSkin = GameController.Instance.AllSkins[random.Next(GameController.Instance.AllSkins.Count)];
            var skin = Instantiate(randomSkin.DozerSkin, visualPoint);
        }
        else
        {
            var skinName = GameController.Instance.RegisterSystem.GetDataAsString(MarketSystem.SelectedSkin);
            var skinScriptable = GameController.Instance.AllSkins.First(skin => skin.ItemID == skinName);
            var skinGO = Instantiate(skinScriptable.DozerSkin, visualPoint);
        }
    }
}
