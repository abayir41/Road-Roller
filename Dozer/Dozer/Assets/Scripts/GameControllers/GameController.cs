using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;


public class GameController : MonoBehaviour
{
    public static GameController Instance;
    [SerializeField] private Config config;

    public GameStatus Status { get; private set; }

    //Dozer Movement and process 
    [Header("Dozer Settings")]
    [SerializeField] private GameObject playerDozer;
    [SerializeField] private GameObject aiDozer;
    private List<Transform> _dozerFollowers;
    private Transform _dozerTrans;
    public string DozerTag => config.DozerTag;

    //Coloring Building, objects ...
    public Dictionary<IColorChanger, Dictionary<int, Color>> RandomlyChangedMaterialsListAndColours { get; private set; }

    //Camera Movement
    [Header("Camera Movement")]
    [SerializeField] private int cameraDistanceDivider;
    private GameObject _cameraGameObject;
    private Transform _cameraTrans;
    private Camera _camera;
    private Vector3 _cameraFarFromDozer;

    //Transparency System
    public string HouseTag => config.HouseTag;
    private GameObject _fadedHouse;
    private bool _houseTriggered;
    
    //Collision System
    public List<string> CollisionObjectFilter => config.CollisionObjectFilter;
    public List<int> DestroyThresholds => config.DestroyThresholds;
    public List<int> ObjectHitPoints => config.ObjectHitPoints;
    public List<int> ObjectDestroyWait => config.ObjectDestroyWait;
    
    //ScoreSystem
    private ScoreSystem _scoreSystem;
    public List<int> LevelThreshold => config.LevelThresholds;
    public List<int> RewardPoints => config.RewardPoints;
    private static int MaxCrashPoint => PlayerController.Player.MaxGrow;
    public int StartScore => config.StartScore;
    
    //LeaderBoardSystem
    public LeaderBoardSystem LeaderBoard { get; private set; }
    
    public List<SkinScriptable> AllSkins => MarketSystem.Instance.DozerSkins;

    [Header("Game Settings")] 
    [SerializeField] private int playerCount;
    [SerializeField] private List<Transform> spawnPoints;
    
    private void Awake()
    {
        
        if (Instance == null)
            Instance = this;
 
        RandomlyChangedMaterialsListAndColours = new Dictionary<IColorChanger, Dictionary<int, Color>>();
        _dozerFollowers = new List<Transform>();
        
        //Caching
        if (!(Camera.main is null)) _cameraGameObject = Camera.main.gameObject;
        _cameraTrans = _cameraGameObject.transform;
        _camera = _cameraGameObject.GetComponent<Camera>();
        LeaderBoard = GetComponent<LeaderBoardSystem>();

        if (RegisterSystem.Instance.GetDataAsString(MarketSystem.SelectedSkin) == "") 
            RegisterSystem.Instance.SaveData(MarketSystem.SelectedSkin,config.BaseSkinID);
    }

    private void Start()
    {
        PrepareScene();
        ActionSys.GameStatusChanged?.Invoke(GameStatus.WaitingOnMenu);
    }

    private void Update()
    {
        if(LeaderBoard.PlayerCount == 1) 
            ActionSys.GameStatusChanged?.Invoke(GameStatus.Ended);
        if(Status != GameStatus.Playing) return;
        
        
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
        ActionSys.GameStatusChanged += GameStatusChanged;
    }
    
    private void OnDisable()
    {
        ActionSys.ObjectGotHit -= Interaction;
        ActionSys.LevelUpped -= LevelUpped;
        ActionSys.GameStatusChanged -= GameStatusChanged;
    }
    
    private void GameStatusChanged(GameStatus status)
    {
        Status = status;

        switch (status)
        {
            case GameStatus.WaitingOnMenu:
                break;
            case GameStatus.Playing:
                break;
            case GameStatus.Paused:
                break;
            case GameStatus.Ended:
                break;
            default:
                break;
        }
    }

    private void LevelUpped(int reward)
    {
        if(PlayerController.Player.Score >= MaxCrashPoint) return;
        StartCoroutine(CameraDistanceIncrease(reward / 3f));
    }
    
    private void Interaction(IInteractable interactable)
    {
        if(PlayerController.Player.Score >= MaxCrashPoint) return;
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
        foreach (var ray in _dozerFollowers.Select(followerTrans => _camera.ScreenPointToRay(_camera.WorldToScreenPoint(followerTrans.position))))
        {
            if (Physics.Raycast(ray, out var hit) && hit.collider.gameObject.CompareTag(HouseTag))
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

    private void PrepareScene()
    {
        SpawnCars();
        
        ActivateSkinSystem();
        
        PaintEnvironment();
    }
    
    private void SpawnCars()
    {
        for (var i = 0; i < playerCount; i++)
        {
            var ranInt = Random.Range(0, spawnPoints.Count);
            
            if (i == 0) //Spawning normal dozer and caching some variebles
            {
                var normalDozer = Instantiate(playerDozer);
                normalDozer.transform.position = spawnPoints[ranInt].position;
                
                _dozerTrans = normalDozer.transform;
                
                var dozerFollowers = GameObject.Find("Dozer_Followers");
                for (var j = 0; j < dozerFollowers.transform.childCount; j++)
                    _dozerFollowers.Add(dozerFollowers.transform.GetChild(j));
                
                var cameraPoint = GameObject.Find("Camera Point");
                _cameraTrans.position = cameraPoint.transform.position;
                _cameraFarFromDozer = _cameraTrans.position - _dozerTrans.position;
                _cameraTrans.LookAt(normalDozer.transform.position);
                
                spawnPoints.Remove(spawnPoints[ranInt]);
            }
            else //Spawning AI 
            {
                var dozerAI = Instantiate(aiDozer);
                dozerAI.transform.position = spawnPoints[ranInt].position;
                spawnPoints.Remove(spawnPoints[ranInt]);
            }
            
        }
    }

    private void ActivateSkinSystem()
    {
        foreach (var skinSystem in FindObjectsOfType<SkinSystem>())
        {
            skinSystem.enabled = true;
        }
    }

    private void PaintEnvironment()
    {
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
    }
}

