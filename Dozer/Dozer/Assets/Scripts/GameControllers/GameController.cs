using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

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

    public static int UnlockedSkinIndex { get; private set; } = 1;
    public static int TotalScore
    {
        get => RegisterSystem.Instance.GetDataAsInt(GameConfig.TotalScore);
        set => RegisterSystem.Instance.SaveData(GameConfig.TotalScore, value);
    }
    
    public static float SkinUnlockProgressPercentage
    {
        get => RegisterSystem.Instance.GetDataAsFloat(GameConfig.SavedProgressSkinUnlock);
        set => RegisterSystem.Instance.SaveData(GameConfig.SavedProgressSkinUnlock,value);
    }

    public int TimeLeft => GameConfig.MatchTimeAsSecond - (int) _timer;
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

            if (TimeLeft == GameConfig.MatchTimeAsSecond / 2)
            {
                ActionSys.GameStatusChanged?.Invoke(GameStatus.Paused);
            }

            if (TimeLeft == 0)
            {
                ActionSys.GameStatusChanged?.Invoke(GameStatus.Ended);
            }
        }
        else if (Mode == GameMode.BeTheLast)
        {
            if(LeaderboardsAbstract.Instance.AlivePlayerCount == 1) 
                ActionSys.GameStatusChanged?.Invoke(GameStatus.Ended);
        }

        
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

        if (status == GameStatus.Playing && TimeLeft == 0)
        {
            _timer /= 2;
        }
        else if (status == GameStatus.Playing && TimeLeft == GameConfig.MatchTimeAsSecond / 2)
        {
            _timer += 1.0f;
        }
    }
    
    private void GameModeChanged(GameMode obj)
    {
        Mode = obj;
    }



    #endregion

    
    public bool IsThereAnyNewSkin()
    {
        if (GameConfig.DozerSkins.Count == UnlockedSkinIndex)
        {
            return false;
        }

        return true;
    }
    public bool NewSkinUnlocked()
    {
        if (GameConfig.DozerSkins[UnlockedSkinIndex].ScoreThreshold <= TotalScore)
        {
            return true;
        }
        return false;
    }

    public float PercentageCalculator()
    {

        var result = TotalScore >= GameConfig.DozerSkins[UnlockedSkinIndex].ScoreThreshold
            ? 100 : 
            ((float)(TotalScore - GameConfig.DozerSkins[UnlockedSkinIndex - 1].ScoreThreshold) / 
             (GameConfig.DozerSkins[UnlockedSkinIndex].ScoreThreshold - GameConfig.DozerSkins[UnlockedSkinIndex - 1].ScoreThreshold)) * 100;
        
        return result;
    }

}
