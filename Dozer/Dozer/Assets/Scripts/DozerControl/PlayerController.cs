using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Player;
    public CarActionSys ActionSysCar;
    [SerializeField] private bool isAI = true;
    
    private ScoreSystem _scoreSystem;
    public int TotalCrashPoint
    {
        get { return _scoreSystem.CurrentScore; }
    }
    public int CurrentLevel
    {
        get { return _scoreSystem.CurrentLevel; }
    }
    public float RatioOfBetweenLevels
    {
        get { return _scoreSystem.RatioOfBetweenLevels(); }
    }

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
        ActionSysCar.MaxLevelReached += MaxLevelReached;
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
        _scoreSystem = new ScoreSystem(ActionSysCar,GameController.Instance.LevelThreshold, GameController.Instance.RewardPoints,GameController.Instance.MaxCrashPoint);
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
    private readonly List<int> _rewardPoints;
    private int _currentLevel = 1;
    private int _currentScore;
    private readonly int _maxScore;
    private bool _maxLevelReached;
    private CarActionSys _carActionSys;
    
    public ScoreSystem(CarActionSys carActionSys,List<int> levelThresholds,List<int> rewardPoints,int maxScore)
    {
        _carActionSys = carActionSys;
        _rewardPoints = rewardPoints;
        _levelThresholds = levelThresholds;
        _maxScore = maxScore;
    }

    public float RatioOfBetweenLevels()
    {
        if (_maxLevelReached) return 0f;
        var diffBetweenLevel = _levelThresholds[_currentLevel] - _levelThresholds[_currentLevel - 1];
        var ourPoint = _currentScore - _levelThresholds[_currentLevel - 1];
        var result = (float) ourPoint / diffBetweenLevel;
        return result;
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
                _carActionSys.LevelUpped(_rewardPoints[_currentLevel - 1]);
                _currentLevel += 1;
                if (_currentLevel == _levelThresholds.Count)
                {
                    _maxLevelReached = true;
                    _carActionSys.MaxLevelReached();
                    break;
                }
                
                
            }
        }
        
        if (_currentScore >= _maxScore)
        {
            _currentScore = _maxScore;
        }
    }
}

public class CarActionSys
{
    public Action<IInteractable> ObjectGotHit;

    public Action<int> LevelUpped;

    public Action MaxLevelReached;

}
