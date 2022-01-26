using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameController : MonoBehaviour
{
    public static GameController Instance;
    
    //Dozer Movement and process 
    [Header("Dozer Settings")]
    [SerializeField] private GameObject dozerGameObject;
    [SerializeField] private List<Transform> dozerFollowers;
    private Transform _dozerTrans;
    public const string DozerTag = "Roller";

    //Coloring Building, objects ...
    public Dictionary<IColorChanger, Dictionary<int, Color>> RandomlyChangedMaterialsListAndColours { get; private set; }

    //Camera Movement
    [Header("Camera Movement")]
    [SerializeField] private Transform focusPoint;
    [SerializeField] private int cameraDistanceDivider;
    private Camera _camera;
    private Transform _cameraTrans;
    private Vector3 _cameraFarFromDozer;

    //Transparency System
    [Header("Transparency Settings")]
    [SerializeField] private string houseTag = "House";
    private GameObject _fadedHouse;
    private bool _houseTriggered;
    
    //Collision System
    [Header("Collision Settings")]
    [SerializeField] public List<string> collisionObjectFilter;
    
    //ScoreSystem
    [Header("Score System")]
    private ScoreSystem _scoreSystem;
    [SerializeField] private int startScore;
    [SerializeField] private List<int> levelThresholds; //This has to begin with 0
    [SerializeField] private List<int> rewardPoints;
    public List<int> LevelThreshold => levelThresholds;
    public List<int> RewardPoints => rewardPoints;
    private int MaxCrashPoint => PlayerController.Player.MaxGrow;
    public int StartScore => startScore;
    
    //LeaderBoardSystem
    [HideInInspector]
    public LeaderBoardSystem leaderBoard;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
 
        RandomlyChangedMaterialsListAndColours = new Dictionary<IColorChanger, Dictionary<int, Color>>();
        //Paint the all buildings, cars, trees...
        var colorableObjs = FindObjectsOfType<ColorChanger>().ToList();
        foreach (var colorableObj in colorableObjs)
        {
            var materialIndexes = colorableObj.gameObject.GetComponent<IRandomlyPaintedMaterialIndex>().MaterialIndexes;
            foreach (var materialIndex in materialIndexes)
            {
                var colorChangerRandomly = colorableObj.gameObject.GetComponent<IColorChangerRandomly>();
                var colors = colorChangerRandomly.Colors;
                var randomInt = Random.Range(0, colors.Length);
                colorChangerRandomly.ChangeColor(colors[randomInt],materialIndex);
                RandomlyChangedMaterialsListAndColours.Add(colorChangerRandomly,new Dictionary<int, Color>(){{materialIndex,colors[randomInt]}});
            }
        }

        //Getting Camera 
        if (!(Camera.main is null)) _camera = Camera.main; 
        _cameraTrans = _camera.transform;
        _cameraTrans.LookAt(focusPoint);

        //Caching
        _dozerTrans = dozerGameObject.transform;
        var dozerTransPosition = _dozerTrans.position;
        _cameraFarFromDozer = _cameraTrans.position - dozerTransPosition;
        leaderBoard = GetComponent<LeaderBoardSystem>();

    }

    private void Update()
    {
        var dozerTransPosition = _dozerTrans.position;
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
        ActionSys.LevelUpped += LevelUpped;
        ActionSys.MaxLevelReached += () => {Debug.Log("MaxLevelReached");};
    }
    
    private void OnDisable()
    {
        ActionSys.ObjectGotHit -= Interaction;
        ActionSys.LevelUpped -= LevelUpped;
    }

    private void LevelUpped(int reward)
    {
        StartCoroutine(CameraDistanceIncrease(reward / 3f));
    }
    
    private void Interaction(IInteractable interactable)
    {
       StartCoroutine(CameraDistanceIncrease(interactable.ObjectHitPoint));
    }
    
    private IEnumerator CameraDistanceIncrease(float distance)
    {
        float timeElapsed = 0;
        
        var goalScale = _cameraFarFromDozer.normalized * distance / cameraDistanceDivider;

        var cachedGrow = Vector3.zero;

        while (timeElapsed < 0.2f)
        {

            
            var lerpRatio = timeElapsed / 0.2f;

            var newGrow = Vector3.Lerp(Vector3.zero, goalScale, lerpRatio);
            
            _cameraFarFromDozer += newGrow - cachedGrow;
            cachedGrow = newGrow;
            
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        
    }

    private void AlphaChanger(GameObject obj,float alphaAmount)
    {

        var randomlyChangedMaterialIndexes = new List<int>();
        var houseRenderer = obj.GetComponent<Renderer>();

        var colorChanger = obj.GetComponent<IColorChanger>();
        if (RandomlyChangedMaterialsListAndColours.ContainsKey(colorChanger))
        {
            foreach (var indexColorPair in RandomlyChangedMaterialsListAndColours[colorChanger])
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
        foreach (var followerTrans in dozerFollowers)
        {
            var ray = _camera.ScreenPointToRay(_camera.WorldToScreenPoint(followerTrans.position));
            if (Physics.Raycast(ray, out var hit) && hit.collider.gameObject.CompareTag(houseTag))
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

