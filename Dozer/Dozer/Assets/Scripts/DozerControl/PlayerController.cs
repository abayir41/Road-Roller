using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Player;

    public Player PlayerProperty { get; set; }
    public string PlayerName => PlayerProperty.Name;
    public CarActionSys ActionSysCar { get; private set; }

    public bool IsAI => isAI;
    [SerializeField] private bool isAI = true;
    
    public int MaxGrow => maxGrow;
    [SerializeField] private int maxGrow;

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
        
        GetComponent<CarSystem>().enabled = true;
    }

    private void Start()
    {
        _scoreSystem = new ScoreSystem(ActionSysCar, GameController.Instance.LevelThreshold, GameController.Instance.RewardPoints, PlayerProperty);
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
        PlayerProperty.IsDead = true;
    }
}



