using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameController : MonoBehaviour
{
    public static GameController Instance;
    [SerializeField] private Config config;

    public GameStatus Status { get; private set; }
    public GameMode Mode { get; private set; }

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
    public List<int> DestroyThresholds => RequiredLevels.ConvertAll(requiredLevel => LevelThreshold[requiredLevel - 1]);
    public List<int> RequiredLevels => config.DestroyThresholdsFromLevels;
    public List<int> ObjectHitPoints => config.ObjectHitPoints;
    public List<int> ObjectDestroyWait => config.ObjectDestroyWait;
    
    //ScoreSystem
    public List<int> LevelThreshold => config.LevelThresholds;
    public List<int> RewardPoints => config.RewardPoints;
    private static int MaxGrowPoint => PlayerController.Player.MaxGrow;
    public int StartScore => config.StartScore;
    
    //Market System
    public List<SkinScriptable> AllSkins => MarketSystem.Instance.DozerSkins;

    [Header("Game Settings")] 
    [SerializeField] private int playerCount;
    [SerializeField] private List<Transform> spawnPoints;
    public List<Player> Players { get; set; }
    
    
    //GameMode Settings
    public int TimeLeft => config.MatchTimeAsSecond - (int) _timer;
    private float _timer;


    private void Awake()
    {
        
        if (Instance == null)
            Instance = this;
 
        RandomlyChangedMaterialsListAndColours = new Dictionary<IColorChanger, Dictionary<int, Color>>();
        _dozerFollowers = new List<Transform>();
        Players = new List<Player>();

        //Caching
        if (!(Camera.main is null)) _cameraGameObject = Camera.main.gameObject;
        _cameraTrans = _cameraGameObject.transform;
        _camera = _cameraGameObject.GetComponent<Camera>();

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
        
        if(Status != GameStatus.Playing) return;
     
        if (Mode == GameMode.TimeCounting)
        {
            _timer += Time.deltaTime;
        }
        else if (Mode == GameMode.BeTheLast)
        {
            
        }
        
        if(LeaderboardsAbstract.Instance.TotalPlayerCount == 1) 
            ActionSys.GameStatusChanged?.Invoke(GameStatus.Ended);
        
        
        var dozerTransPosition = _dozerTrans.position;
        ObjectFollower(_cameraFarFromDozer,dozerTransPosition,_cameraTrans);

        
        _fadedHouse = Looking_Any_Big_House();
        
        if (!(_fadedHouse is null) && !_houseTriggered)
        {
            _houseTriggered = true;
            AlphaChanger(_fadedHouse,0.3f);
        }
        
    }

    #region Subscription

    private void OnEnable()
    {
        ActionSys.ObjectGotHit += Interaction;
        ActionSys.LevelUpped += LevelUpped;
        ActionSys.MaxLevelReached += () => {Debug.Log("MaxLevelReached");};
        ActionSys.GameStatusChanged += GameStatusChanged;
        ActionSys.GameModeChanged += GameModeChanged;
    }
    
    private void OnDisable()
    {
        ActionSys.ObjectGotHit -= Interaction;
        ActionSys.LevelUpped -= LevelUpped;
        ActionSys.GameStatusChanged -= GameStatusChanged;
        ActionSys.GameModeChanged -= GameModeChanged;
    }

    #endregion

    #region Subs Methods
    
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
        }
    }

    private void LevelUpped(int reward)
    {
        if(PlayerController.Player.Score >= MaxGrowPoint) return;
        StartCoroutine(CameraDistanceIncrease(reward / 3f));
    }
    
    private void Interaction(IInteractable interactable)
    {
        if(PlayerController.Player.Score >= MaxGrowPoint) return;
        StartCoroutine(CameraDistanceIncrease(interactable.ObjectHitPoint));
    }
    
    private void GameModeChanged(GameMode mode)
    {
        Mode = mode;

        switch (mode)
        {
            case GameMode.TimeCounting:
                break;
            case GameMode.BeTheLast:
                break;
        }
    }


    #endregion

    #region Animations

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

    #endregion
    
    #region Game Dynamic Methods

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

    #endregion
    
    #region Scene Preparing

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
                
                RegisterTheDozer(normalDozer, "You");
                SetDozerFollowers();
                
                normalDozer.transform.position = spawnPoints[ranInt].position; //setting position
                _dozerTrans = normalDozer.transform; //caching dozer transform

                SetTheCamera();
                
                spawnPoints.Remove(spawnPoints[ranInt]);
            }
            else //Spawning AI 
            {
                var dozerAI = Instantiate(aiDozer);
                
                RegisterTheDozer(dozerAI, LeaderboardsAbstract.Instance.GetRandomName());
                
                dozerAI.transform.position = spawnPoints[ranInt].position;
                spawnPoints.Remove(spawnPoints[ranInt]);
            }
            
        }
    }

    private void RegisterTheDozer(GameObject dozer, string playerName)
    {
        var player = new Player(playerName, StartScore, LeaderboardsAbstract.Instance.GetRandomColor());
        dozer.GetComponent<PlayerController>().PlayerProperty = player;
        Players.Add(player);
        LeaderboardsAbstract.Instance.AddPlayer(playerName, player);
    }

    private void SetDozerFollowers()
    {
        var dozerFollowers = GameObject.Find("Dozer_Followers");
        for (var j = 0; j < dozerFollowers.transform.childCount; j++)
            _dozerFollowers.Add(dozerFollowers.transform.GetChild(j));
    }

    private void SetTheCamera()
    {
        var cameraPoint = GameObject.Find("Camera Point");
        _cameraTrans.position = cameraPoint.transform.position;
        var position = _dozerTrans.position;
        _cameraFarFromDozer = _cameraTrans.position - position;
        _cameraTrans.LookAt(position);
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

    #endregion

}

