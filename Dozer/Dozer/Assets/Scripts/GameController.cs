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
    public static string DozerTag = "Roller";
    [SerializeField] private GameObject dozerGameObject;
    [SerializeField] private List<Transform> dozerFollowers;
    private Transform _dozerTrans;
    private Dictionary<Transform,Vector3> _dozerFollowersAndFarFromDozer;
    
    //Coloring Building, objects ...
    public Dictionary<IColorChanger, Dictionary<int, Color>> randomlyChangedMaterialsListAndColours =>
        _randomlyChangedMaterialsListAndColours;
    private Dictionary<IColorChanger, Dictionary<int, Color>> _randomlyChangedMaterialsListAndColours;
    
    //Camera Movement
    [SerializeField] private int cameraDistanceDivider;
    private Camera _camera;
    private Transform _cameraTrans;
    private Vector3 _cameraFarFromDozer;
    
    //Transparency System
    [SerializeField] private string houseTag = "House";
    private GameObject _fadedHouse;
    private bool _houseTriggered;
    
    //ScoreSystem
    private ScoreSystem _scoreSystem;
    [SerializeField] private List<int> levelThresholds; //This has to begin with 0
    [SerializeField] private int multipleStepPointConstant;
    [SerializeField] private int totalCrashPoint;
    [SerializeField] private int maxCrashPoint;
    public int TotalCrashPoint
    {
        get { return totalCrashPoint; }
    }
    public int MultipleStepPointConstant
    {
        get { return multipleStepPointConstant; }
    }
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        _randomlyChangedMaterialsListAndColours = new Dictionary<IColorChanger, Dictionary<int, Color>>();
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

        _scoreSystem = new ScoreSystem(levelThresholds, maxCrashPoint);
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
        ActionSys.LevelUpped += LevelUpped;
        ActionSys.MaxLevelReached += () => Debug.Log("MaxLevelReached");
        ActionSys.MaxScoreReached += () => Debug.Log("MaxScoreReached");
    }
    
    private void OnDisable()
    {
        ActionSys.ObjectGotHit -= Interaction;
        ActionSys.LevelUpped -= LevelUpped;
    }

    void LevelUpped()
    {
        var score = _scoreSystem.CurrentScore;
        var level = _scoreSystem.CurrentLevel;
        Debug.Log("Level Upped " + score + " " + level);
    }
    
    private void Interaction(IInteractable interactable)
    {
        _scoreSystem.AddScore(interactable.ObjectHitPoint);
       StartCoroutine(CameraDistanceIncrease(interactable.ObjectHitPoint));
    }
    
    private IEnumerator CameraDistanceIncrease(float distance)
    {
        float timeElapsed = 0;

        var newDistance = _cameraFarFromDozer+_cameraFarFromDozer.normalized * distance / cameraDistanceDivider;
        
        var cachedFar = _cameraFarFromDozer;

        while (timeElapsed < 0.2f)
        {
            var lerpRatio = timeElapsed / 0.2f;
            _cameraFarFromDozer = Vector3.Lerp(cachedFar, newDistance, lerpRatio);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        
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

public class ScoreSystem
{
    public int CurrentLevel
    {
        get { return _currentLevel; }
    }

    public int CurrentScore
    {
        get { return _currentScore; }
    }
    
    private readonly List<int> _levelThresholds;
    private int _currentLevel = 1;
    private int _currentScore = 0;
    private readonly int _maxScore;
    private bool _maxLevelReached;
    
    public ScoreSystem(List<int> levelThresholds,int maxScore)
    {
        _levelThresholds = levelThresholds;
        _maxScore = maxScore;
    }

    public void AddScore(int score)
    {
        if(_currentScore >= _maxScore)
            return;
        
        _currentScore += score;

        if (!_maxLevelReached)
        {
            while (_currentScore >= _levelThresholds[_currentLevel])
            {
                _currentLevel += 1;
                if (_currentLevel == _levelThresholds.Count)
                {
                    _maxLevelReached = true;
                    ActionSys.MaxLevelReached();
                    break;
                }
                else
                {
                    ActionSys.LevelUpped();
                }
            }
        }
        
        if (_currentScore >= _maxScore)
        {
            _currentScore = _maxScore;
            ActionSys.MaxScoreReached();
        }
    }
}
