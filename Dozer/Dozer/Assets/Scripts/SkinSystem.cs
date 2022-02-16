using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkinSystem : MonoBehaviour
{
    private GameObject _spawnedSkin;
    private Transform visualPoint;
    private void Start()
    {
        visualPoint = GetComponent<CarSystem>().VisualPoint;
        var isAI = GetComponent<PlayerController>().IsAI;
        var interactableObj = GetComponent<InteractableObj>();
        if (isAI)
        {
            var ranInt = Random.Range(0, GameController.GameConfig.DozerSkins.Count);
            var randomSkin = GameController.GameConfig.DozerSkins[ranInt];
            _spawnedSkin = Instantiate(randomSkin.DozerSkin, visualPoint);
        }
        else
        {
            var skinIndex = RegisterSystem.Instance.GetDataAsInt(GameController.GameConfig.SelectedSkinIndexString);
            var skinScriptable = GameController.GameConfig.DozerSkins[skinIndex];
            _spawnedSkin = Instantiate(skinScriptable.DozerSkin, visualPoint);
        }

        var meshRenderers = _spawnedSkin.GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRenderer in meshRenderers)
        {
            interactableObj.meshRenderers.Add(meshRenderer);
        }
    }

    private void OnEnable()
    {
        var isAI = GetComponent<PlayerController>().IsAI;
        if (isAI) return;

        ActionSys.SkinSelected += RefreshSkin;
    }

    private void OnDisable()
    {
        var isAI = GetComponent<PlayerController>().IsAI;
        if (isAI) return;

        ActionSys.SkinSelected -= RefreshSkin;
    }

    void RefreshSkin(int ID)
    {
        Destroy(_spawnedSkin);
        _spawnedSkin = Instantiate(GameController.GameConfig.DozerSkins[ID].DozerSkin, visualPoint);
    }
}
