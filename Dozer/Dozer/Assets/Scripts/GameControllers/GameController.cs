using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    #region Config

    public static GameConfig GameConfig;
    [SerializeField] private GameConfig config;
    public static List<int> DestroyThresholds => GameConfig.DestroyThresholdsFromLevels.ConvertAll(requiredLevel => GameConfig.LevelThresholds[requiredLevel - 1]);
    
    #endregion

    #region Status
    
    public static GameStatus Status { get; private set; }
    public static GameMode Mode { get; private set; }

    #endregion

    public static List<SkinScriptable> AllSkins => MarketSystem.Instance.DozerSkins;

    private float _timer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        GameConfig = config;
    }

    private void Update()
    {
        if(Status != GameStatus.Playing) return;
        
        if (Mode == GameMode.TimeCounting)
        {
            _timer += Time.deltaTime;
        }
        else if (GameController.Mode == GameMode.BeTheLast)
        {

        }

        if(LeaderboardsAbstract.Instance.TotalPlayerCount == 1) 
            ActionSys.GameStatusChanged?.Invoke(GameStatus.Ended);
    }

    #region Subscription

    private void OnEnable()
    {
        ActionSys.GameStatusChanged += GameStatusChanged;
        ActionSys.GameModeChanged += GameModeChanged;
    }
    
    private void OnDisable()
    {
        ActionSys.GameStatusChanged -= GameStatusChanged;
        ActionSys.GameModeChanged -= GameModeChanged;
    }
    
    private void GameStatusChanged(GameStatus status)
    {
        Status = status;
    }
    
    private void GameModeChanged(GameMode obj)
    {
        Mode = obj;
    }

    #endregion
    
}
