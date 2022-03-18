using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkinSystem : MonoBehaviour
{
    private GameObject _spawnedSkin;
    private Transform _visualPoint;
    private InteractableObj _obj;
    private void Start()
    {
        _visualPoint = GetComponent<CarSystem>().VisualPoint;
        var isAI = GetComponent<PlayerController>().IsAI;
        _obj = GetComponent<InteractableObj>();
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

        _spawnedSkin.transform.localPosition = Vector3.zero;
        _spawnedSkin.transform.localEulerAngles = Vector3.zero;
        _spawnedSkin.transform.localScale = Vector3.one;
        
        _obj.meshRenderers = _spawnedSkin.GetComponentsInChildren<MeshRenderer>().ToList();
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
        _spawnedSkin.transform.localPosition = Vector3.zero;
        _spawnedSkin.transform.localEulerAngles = Vector3.zero;
        _spawnedSkin.transform.localScale = Vector3.one;
        
        _obj.meshRenderers = _spawnedSkin.GetComponentsInChildren<MeshRenderer>().ToList();
    }
}
