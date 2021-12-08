using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public static string DozerTag = "Roller";
    
    [SerializeField] private string houseTag = "House";
    [SerializeField] private GameObject dozerGameObject;
    [SerializeField] private List<Transform> dozerFollowers;
    
    [SerializeField] private int multipleStepPointConstant;
    [SerializeField] private int maxCrashPoint;
    public int MaxCrashPoint
    {
        get { return maxCrashPoint; }
    }
    
    public int MultipleStepPointConstant
    {
        get { return multipleStepPointConstant; }
    }
    
    private Dictionary<Transform,Vector3> _dozerFollowersAndFarFromDozer;

    public Dictionary<IColorChanger, Dictionary<int, Color>> randomlyChangedMaterialsListAndColours =>
        _randomlyChangedMaterialsListAndColours;
    
    private Dictionary<IColorChanger, Dictionary<int, Color>> _randomlyChangedMaterialsListAndColours;
    private Vector3 _cameraFarFromDozer;
    private Transform _dozerTrans;
    private Transform _cameraTrans;
    private Camera _camera;

    private GameObject _fadedHouse;
    private bool _houseTriggered;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        _randomlyChangedMaterialsListAndColours = new Dictionary<IColorChanger, Dictionary<int, Color>>();
        //Paint the all buildings, cars, trees...
        var paintableObjs = FindObjectsOfType<ColorChanger>().ToList();
        foreach (var paintableObj in paintableObjs)
        {
            var materialIndexes = paintableObj.gameObject.GetComponent<IRandomlyPaintedMaterialIndex>().MaterialIndexes;
            foreach (var materialIndex in materialIndexes)
            {
                var colorChangerRandomly = paintableObj.gameObject.GetComponent<IColorChangerRandomly>();
                var colors = colorChangerRandomly.Colors;
                var randomInt = Random.Range(0, colors.Length);
                colorChangerRandomly.ChangeColor(colors[randomInt],materialIndex);
                _randomlyChangedMaterialsListAndColours.Add(colorChangerRandomly,new Dictionary<int, Color>(){{materialIndex,colors[randomInt]}});
            }
        }

        //Getting Camera 
        if (!(Camera.main is null)) _camera = Camera.main; 
        _cameraTrans = _camera.transform;
        
        //Caching
        _dozerTrans = dozerGameObject.transform;
        var dozerTransPosition = _dozerTrans.position;
        _cameraFarFromDozer = _cameraTrans.position - dozerTransPosition;

        _dozerFollowersAndFarFromDozer = new Dictionary<Transform, Vector3>();
        dozerFollowers.ForEach(followerTrans =>
        {
            var dozerFollowerFarFromDozer = followerTrans.position - dozerTransPosition;
            _dozerFollowersAndFarFromDozer.Add(followerTrans,dozerFollowerFarFromDozer);
        });
    }

    private void Update()
    {
        var dozerTransPosition = _dozerTrans.position;
        foreach (var keyValuePair in _dozerFollowersAndFarFromDozer)
        {
            var followerTrans = keyValuePair.Key;
            var distance = keyValuePair.Value;
            ObjectFollower(distance,dozerTransPosition,followerTrans);
        }
        ObjectFollower(_cameraFarFromDozer,dozerTransPosition,_cameraTrans);

        
        _fadedHouse = Looking_Any_Big_House();
        
        if (!(_fadedHouse is null) && !_houseTriggered)
        {
            _houseTriggered = true;
            AlphaChanger(_fadedHouse,0.3f);
        }
        
    }
    
    private void OnEnable()
    {
        ActionSys.ObjectGotHit += Interaction;
    }
    
    private void OnDisable()
    {
        ActionSys.ObjectGotHit += Interaction;
    }

    private void Interaction(IInteractable interactable)
    {
        Debug.Log("Controller Interaction");
    }

    private void AlphaChanger(GameObject obj,float alphaAmount)
    {

        var randomlyChangedMaterialIndexes = new List<int>();
        var houseRenderer = obj.GetComponent<Renderer>();

        var colorChanger = obj.GetComponent<IColorChanger>();
        if (_randomlyChangedMaterialsListAndColours.ContainsKey(colorChanger))
        {
            foreach (var indexColorPair in _randomlyChangedMaterialsListAndColours[colorChanger])
            {
                var color = indexColorPair.Value;
                color.a = alphaAmount;
                colorChanger.ChangeColor(color, indexColorPair.Key);
                randomlyChangedMaterialIndexes.Add(indexColorPair.Key);
            }
        }

        var materials = houseRenderer.sharedMaterials;
        for (var i = 0; i < materials.Length; i++)
        {
            if (randomlyChangedMaterialIndexes.Contains(i)) continue;
            var material = materials[i];
            var color = material.color;
            color.a = alphaAmount;
            colorChanger.ChangeColor(color, i);
        }
    }

    private static void ObjectFollower(Vector3 distance, Vector3 from, Transform obj)
    {
        obj.position = distance + from;
    }
    
    private GameObject Looking_Any_Big_House()
    {
        GameObject savedGameObject = null;
        foreach (var keyValuePair in _dozerFollowersAndFarFromDozer)
        {
            var followerTrans = keyValuePair.Key;
            var distance = keyValuePair.Value;
            Ray ray = _camera.ScreenPointToRay(_camera.WorldToScreenPoint(followerTrans.position));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag(houseTag))
            {
                savedGameObject = hit.collider.gameObject;
            }
            else
            {
                if (_houseTriggered)
                {
                    AlphaChanger(_fadedHouse,1f);
                    _houseTriggered = false;
                }
                return null;
            }
               
        }

        return savedGameObject;

    }
}
