using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Player { get; private set; }

    //Player Properties and references
    public Player PlayerProperty { get; set; }
    public string PlayerName => PlayerProperty.Name;
    public int Level => PlayerProperty.Level;
    public int Score => PlayerProperty.Score;
    
    //Local trigger System
    public CarActionSys ActionSysCar { get; private set; }

    //Dozer Properties
    public bool IsAI => isAI;
    [SerializeField] private bool isAI = true;
    
    public int MaxGrow => maxGrow;
    [SerializeField] private int maxGrow;

    //Score System and References
    private ScoreSystem _scoreSystem;
    public float RatioOfBetweenLevels => _scoreSystem.RatioOfBetweenLevels();

    #region Subscription

    private void OnEnable()
    {
        ActionSysCar.LevelUpped += LevelUpped;
        ActionSysCar.ObjectGotHit += Interaction;
        ActionSysCar.MaxLevelReached += MaxLevelReached;
    }
    
    private void OnDisable()
    {
        ActionSysCar.LevelUpped -= LevelUpped;
        ActionSysCar.ObjectGotHit -= Interaction;
        ActionSysCar.MaxLevelReached -= MaxLevelReached;
    }

    #endregion

    

    private void Awake()
    {
        if (isAI == false)
        {
            Player = this;
        }
        ActionSysCar = new CarActionSys();
        GetComponent<CarController>().enabled = true;
        GetComponent<CarSystem>().enabled = true;
    }

    private void Start()
    {
        _scoreSystem = new ScoreSystem(ActionSysCar, MapController.Instance.mapConfig.LevelThresholds, MapController.Instance.mapConfig.RewardPoints, PlayerProperty);
    }

    #region Subscription Methods

    private void LevelUpped(int obj)
    {
        if (Player == this)
            ActionSys.LevelUpped?.Invoke(obj);
    }

    private void Interaction(IInteractable obj)
    {
        if (obj.IsDozer)
            PlayerProperty.KillCount += 1;
            
        _scoreSystem.AddScore(obj.ObjectHitPoint);

        if (Player == this)
            ActionSys.ObjectGotHit?.Invoke(obj);
    }

    private void MaxLevelReached()
    {
        if (Player == this)
            ActionSys.MaxLevelReached?.Invoke();
    }

    #endregion


    private void OnDestroy()
    {
        PlayerProperty.IsDead = true;

        if (Player == this && GameController.Status == GameStatus.Playing)
        {
            ActionSys.GameStatusChanged?.Invoke(GameStatus.Lost);
        }
    }
}



