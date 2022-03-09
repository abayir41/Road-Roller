using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;


public class MapController : MonoBehaviour
{
    public static MapController Instance { get; private set; }

    //Dozer Movement and process 
    [Header("Dozer Settings")]
    private Transform _dozerTrans;
    //Coloring Building, objects ...
    public Dictionary<IColorChanger, Dictionary<int, Color>> RandomlyChangedMaterialsListAndColours { get; private set; }

    //Camera Movement
    [Header("Camera Movement")] 
    private float _smoothFollowTime;
    private static Transform CameraTrans => GameController.Instance.GameCamera.gameObject.transform;
    private Vector3 _dozerToCameraDirection;
    private Transform _cameraOffsetFromDozer;
    private Vector3 _camVelocity = Vector3.zero;

    public MapConfig mapConfig;
    [Header("Map Settings")] 
    [SerializeField] private List<Transform> spawnPoints;
    
    private void Awake()
    {
        Instance = this;
        RandomlyChangedMaterialsListAndColours = new Dictionary<IColorChanger, Dictionary<int, Color>>();
        _smoothFollowTime = mapConfig.CameraSmoothness;
    }

    private void Start()
    {
        PrepareScene();
    }
    

    private void FixedUpdate()
    {
        if (GameController.Status != GameStatus.Playing) return;
        
        SmoothCameraFollower();
    }

    #region Subscription

    private void OnEnable()
    {
        ActionSys.ObjectGotHit += Interaction;
        ActionSys.LevelUpped += LevelUpped;
    }
    
    private void OnDisable()
    {
        ActionSys.ObjectGotHit -= Interaction;
        ActionSys.LevelUpped -= LevelUpped;
    }

    #endregion

    #region Subs Methods
    

    private void LevelUpped(int reward)
    {
        StartCoroutine(CameraDistanceIncrease(reward,true));
    }
    
    private void Interaction(IInteractable interactable)
    {
        StartCoroutine(CameraDistanceIncrease(interactable.ObjectHitPoint));
    }
    
    #endregion

    #region Animations

    private IEnumerator CameraDistanceIncrease(float distance, bool disableSmoothness = false)
    {
        float timeElapsed = 0;
        
        var goalScale = _dozerToCameraDirection.normalized * distance / mapConfig.CameraDistanceDivider;

        var cachedGrow = Vector3.zero;

        if(disableSmoothness)
            _smoothFollowTime = 0.0001f;

        while (timeElapsed < 0.2f)
        {

            
            var lerpRatio = timeElapsed / 0.2f;

            var newGrow = Vector3.Lerp(Vector3.zero, goalScale, lerpRatio);
            
            _cameraOffsetFromDozer.localPosition += newGrow - cachedGrow;
            cachedGrow = newGrow;
            
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        _smoothFollowTime = mapConfig.CameraSmoothness;

    }

    #endregion
    
    #region Game Dynamic Methods
    private void SmoothCameraFollower()
    {
        
        CameraTrans.position = Vector3.SmoothDamp(CameraTrans.position, _cameraOffsetFromDozer.position,
            ref _camVelocity, _smoothFollowTime);
        
        var position = _dozerTrans.position;
        CameraTrans.LookAt(position);
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
        for (var i = 0; i < mapConfig.PlayerCount; i++)
        {
            var ranInt = Random.Range(0, spawnPoints.Count);
            
            if (i == 0) //Spawning normal dozer and caching some variables
            {
                var normalDozer = Instantiate(mapConfig.PlayerDozer, spawnPoints[ranInt]);
                
                RegisterTheDozer(normalDozer, "You");

                normalDozer.transform.localPosition = Vector3.zero;//setting position
                normalDozer.transform.localEulerAngles = Vector3.zero;//Setting Rot
                normalDozer.transform.localScale = Vector3.one;//setting local scale
                normalDozer.transform.parent = GameInitializer.CurrentMap.transform;
                _dozerTrans = normalDozer.transform; //caching dozer transform
                
                normalDozer.GetComponent<CarController>().SetVelocity(mapConfig.MapStartVelocity,mapConfig.VelocityDivider);
                
                SetTheCamera();
                
                spawnPoints.Remove(spawnPoints[ranInt]);
            }
            else //Spawning AI 
            {
                var dozerAI = Instantiate(mapConfig.AIDozer, spawnPoints[ranInt]);
                
                RegisterTheDozer(dozerAI, LeaderboardsAbstract.Instance.GetRandomName());
                
                dozerAI.transform.localPosition = Vector3.zero;//setting position
                dozerAI.transform.localEulerAngles = Vector3.zero;//Setting Rot
                dozerAI.transform.localScale = Vector3.one;//setting local scale
                dozerAI.transform.parent = GameInitializer.CurrentMap.transform;
                dozerAI.GetComponent<CarController>().SetVelocity(mapConfig.MapStartVelocity,mapConfig.VelocityDivider);
                spawnPoints.Remove(spawnPoints[ranInt]);
            }
            
        }
    }

    private void RegisterTheDozer(GameObject dozer, string playerName)
    {
        var playerController = dozer.GetComponent<PlayerController>();
        var player = new Player(playerName, Instance.mapConfig.StartScore, LeaderboardsAbstract.Instance.GetRandomColor(),playerController.uiPosition);
        playerController.PlayerProperty = player;
        LeaderboardsAbstract.Instance.AddPlayer(player);
    }

    private void SetTheCamera()
    {
        _cameraOffsetFromDozer = GameObject.Find("Camera Point").transform;
        CameraTrans.position = _cameraOffsetFromDozer.position;
        var position = _dozerTrans.position;
        _dozerToCameraDirection = CameraTrans.position - position;
        CameraTrans.LookAt(position);
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
            foreach (var materialIndex in colorableObj.PaintMaterialIndexes)
            {
                var colors = colorableObj.Colors;
                var randomInt = Random.Range(0, colors.Length);
                colorableObj.ChangeColor(colors[randomInt],materialIndex);
                RandomlyChangedMaterialsListAndColours.Add(colorableObj,new Dictionary<int, Color>{{materialIndex,colors[randomInt]}});
            }
        }
    }

    #endregion
}

