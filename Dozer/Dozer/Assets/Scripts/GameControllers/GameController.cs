using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour, ISystem
{
    public static GameController Instance { get; private set; }

    #region Config

    public static GameConfig GameConfig;
    [SerializeField] private GameConfig config;
    public static List<int> DestroyThresholds => MapController.Instance.mapConfig.DestroyThresholdsFromLevels.ConvertAll(requiredLevel => MapController.Instance.mapConfig.LevelThresholds[requiredLevel - 1]);
    
    #endregion

    #region Status
    
    public static GameStatus Status { get; private set; }
    public static GameMode Mode { get; private set; }

    #endregion

    public static int UnlockedSkinIndex
    {
        get => RegisterSystem.Instance.GetDataAsInt(GameConfig.UnlockedSkinString);
        
        private set => RegisterSystem.Instance.SaveData(GameConfig.UnlockedSkinString, value);
    }

    public static int TotalScore
    {
        get => RegisterSystem.Instance.GetDataAsInt(GameConfig.TotalScore);
        private set => RegisterSystem.Instance.SaveData(GameConfig.TotalScore, value);
    }
    
    public static float SkinUnlockProgressPercentage
    {
        get => RegisterSystem.Instance.GetDataAsFloat(GameConfig.SavedProgressSkinUnlock);
        private set => RegisterSystem.Instance.SaveData(GameConfig.SavedProgressSkinUnlock,value);
    }

    public static int SelectedSkinIndex
    {
        get => RegisterSystem.Instance.GetDataAsInt(GameConfig.SelectedSkinIndexString);
        private set => RegisterSystem.Instance.SaveData(GameConfig.SelectedSkinIndexString, value);
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

        if (Status == GameStatus.Playing)
        {
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

            if(LeaderboardsAbstract.Instance.AlivePlayerCount == 1) 
                ActionSys.GameStatusChanged?.Invoke(GameStatus.Ended);
        }
    }

    #region Subscription

    private void OnEnable()
    {
        ActionSys.GameStatusChanged += GameStatusChanged;
        ActionSys.GameModeChanged += GameModeChanged;
        ActionSys.SkinSelected += SelectedSkin;
    }
    
    private void OnDisable()
    {
        ActionSys.GameStatusChanged -= GameStatusChanged;
        ActionSys.GameModeChanged -= GameModeChanged;
        ActionSys.SkinSelected -= SelectedSkin;
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

        if (status == GameStatus.Ended || status == GameStatus.Lost)
        {
            TotalScore += PlayerController.Player.Score;
        }
    }
    
    private void GameModeChanged(GameMode obj)
    {
        Mode = obj;
    }
    
    public void ResetTheSystem()
    {
        _timer = 0;
        if (!IsThereAnyNewSkin())
        {
            return;
        }
        if (NewSkinUnlocked())
        {
            UnlockedSkinIndex += 1;
            SkinUnlockProgressPercentage = 0;
        }
    }
    
    private void SelectedSkin(int id)
    {
        SelectedSkinIndex = id;
    }
    #endregion


    #region GameMethods
    public static bool IsThereAnyNewSkin()
    {
        if (GameConfig.DozerSkins.Count - 1 == UnlockedSkinIndex)
        {
            return false;
        }

        return true;
    }
    public static bool NewSkinUnlocked()
    {
        if (GameConfig.DozerSkins[UnlockedSkinIndex + 1].ScoreThreshold <= TotalScore)
        {
            return true;
        }
        return false;
    }

    public static float PercentageCalculator()
    {

        var result = TotalScore >= GameConfig.DozerSkins[UnlockedSkinIndex + 1].ScoreThreshold
            ? 100 : 
            ((float)(TotalScore - GameConfig.DozerSkins[UnlockedSkinIndex].ScoreThreshold) / 
             (GameConfig.DozerSkins[UnlockedSkinIndex + 1].ScoreThreshold - GameConfig.DozerSkins[UnlockedSkinIndex].ScoreThreshold)) * 100;

        return result;
    }


    public bool IsTimeOutRewardVideoReady()
    {
        Debug.LogWarning("Reward Video Not Implemented");
        return true;
    }

    public bool IsEndLeaderboardVideoReady()
    {
        Debug.LogWarning("Reward Video Not Implemented");
        return true;
    }
    

    #endregion
    

    
    
}
