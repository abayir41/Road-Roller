using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Player;
    
    public CarActionSys ActionSysCar => _actionSysCar;
    private CarActionSys _actionSysCar;
    
    public bool IsAI => isAI;
    [SerializeField] private bool isAI = true;
    
    public int MaxGrow => maxGrow;
    [SerializeField] private int maxGrow;

    public string PlayerName { get; private set; }


    private ScoreSystem _scoreSystem;
    public int Score => _scoreSystem.CurrentScore;
    public float RatioOfBetweenLevels => _scoreSystem.RatioOfBetweenLevels();

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
    

    private void Awake()
    {
        if (isAI == false)
        {
            Player = this;
        }
        _actionSysCar = new CarActionSys();
        
        GetComponent<CarSystem>().enabled = true;
    }

    private void Start()
    {
        PlayerName = isAI ? GameController.Instance.LeaderBoard.GetRandomName() : "You";
        _scoreSystem = new ScoreSystem(ActionSysCar,
            GameController.Instance.LevelThreshold, 
            GameController.Instance.RewardPoints,
            GameController.Instance.StartScore,
            GameController.Instance.LeaderBoard,
            PlayerName);
    }


    private void LevelUpped(int obj)
    {
        if (Player == this)
            ActionSys.LevelUpped(obj);
    }

    private void Interaction(IInteractable obj)
    {
        _scoreSystem.AddScore(obj.ObjectHitPoint);

        if (Player == this)
            ActionSys.ObjectGotHit(obj);
    }

    private void MaxLevelReached()
    {
        if (Player == this)
            ActionSys.MaxLevelReached();
    }

    private void OnDestroy()
    {
        GameController.Instance.LeaderBoard.RemovePlayer(PlayerName);
    }
}



