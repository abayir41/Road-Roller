using UnityEngine;
using Random = UnityEngine.Random;

public class SkinSystem : MonoBehaviour
{
    private GameObject _spawnedSkin;
    private Transform _visualPoint;
    private void Start()
    {
        _visualPoint = GetComponent<CarSystem>().VisualPoint;
        var isAI = GetComponent<PlayerController>().IsAI;
        var interactableObj = GetComponent<InteractableObj>();
        if (isAI)
        {
            var ranInt = Random.Range(0, GameController.GameConfig.DozerSkins.Count);
            var randomSkin = GameController.GameConfig.DozerSkins[ranInt];
            _spawnedSkin = Instantiate(randomSkin.DozerSkin, _visualPoint);
        }
        else
        {
            var skinIndex = RegisterSystem.Instance.GetDataAsInt(GameController.GameConfig.SelectedSkinIndexString);
            var skinScriptable = GameController.GameConfig.DozerSkins[skinIndex];
            _spawnedSkin = Instantiate(skinScriptable.DozerSkin, _visualPoint);
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

    void RefreshSkin(int id)
    {
        Destroy(_spawnedSkin);
        _spawnedSkin = Instantiate(GameController.GameConfig.DozerSkins[id].DozerSkin, _visualPoint);
    }
}
