using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject dozerGameObject;
    private Vector3 _cameraFarFromDozer;
    private Transform _dozerTrans;
    private Transform _camera;
    private ISteerSystem _steerSystem;
    private void Awake()
    {
        //Paint the all buildings, cars, trees...
        var paintableObjs = FindObjectsOfType<ColorChanger>().OfType<IColorChangerRandomly>().ToList();
        foreach (var paintableObj in paintableObjs)
        {
            paintableObj.SelectColorRandomly();
        }

        if (!(Camera.main is null)) _camera = Camera.main.transform;
        _dozerTrans = dozerGameObject.transform;
        _steerSystem = GetComponent<BasicSteerSystem>();
        _cameraFarFromDozer = _camera.position - _dozerTrans.position;
    }

    private void Update()
    {
        _camera.position = _cameraFarFromDozer + _dozerTrans.position;
    }
}
