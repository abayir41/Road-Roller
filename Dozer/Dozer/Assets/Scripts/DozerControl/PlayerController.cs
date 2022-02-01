using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Player;
    public CarActionSys ActionSysCar;
    public bool IsAI => isAI;
    [SerializeField] private bool isAI = true;
    [SerializeField] private int maxGrow;
    public int MaxGrow => maxGrow;
    private string _playerName;
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
        ActionSysCar = new CarActionSys();

        GetComponent<CarController>().enabled = true;
        GetComponent<CarSystem>().enabled = true;
    }

    private void Start()
    {
        _playerName = isAI ? GameController.Instance.leaderBoard.GetRandomName() : "You";
        _scoreSystem = new ScoreSystem(ActionSysCar,
            GameController.Instance.LevelThreshold, 
            GameController.Instance.RewardPoints,
            GameController.Instance.StartScore,
            GameController.Instance.leaderBoard,
            _playerName);
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
        GameController.Instance.leaderBoard.RemovePlayer(_playerName);
    }
}



