using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkinSystem : MonoBehaviour
{
    private void Start()
    {
        var visualPoint = GetComponent<CarSystem>().VisualPoint;
        var isAI = GetComponent<PlayerController>().IsAI;
        var interactableObj = GetComponent<InteractableObj>();
        GameObject skinGameObject;
        if (isAI)
        {
            var ranInt = Random.Range(0, GameController.Instance.AllSkins.Count);
            var randomSkin = GameController.Instance.AllSkins[ranInt];
            skinGameObject = Instantiate(randomSkin.DozerSkin, visualPoint);
        }
        else
        {
            var skinName = GameController.Instance.RegisterSystem.GetDataAsString(MarketSystem.SelectedSkin);
            var skinScriptable = GameController.Instance.AllSkins.First(skin => skin.ItemID == skinName);
            skinGameObject = Instantiate(skinScriptable.DozerSkin, visualPoint);
        }

        var meshRenderers = skinGameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRenderer in meshRenderers)
        {
            interactableObj.meshRenderers.Add(meshRenderer);
        }

        this.enabled = false;
    }
}
