using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Config", menuName = "Config")]
public class Config : ScriptableObject
{
    public string HouseTag => houseTag;
    [SerializeField] private string houseTag;

    public string DozerTag => dozerTag;
    [SerializeField] private string dozerTag;

    public string BaseSkinID => baseSkinID;
    [SerializeField] private string baseSkinID;
    
    public List<string> CollisionObjectFilter => collisionObjectFilter;
    public List<int> DestroyThresholds => destroyThresholds;
    public List<int> ObjectHitPoints => objectHitPoints;
    public List<int> ObjectDestroyWait => objectDestroyWait;
    
    [Header("Collision System")]
    [SerializeField] private List<string> collisionObjectFilter;
    [SerializeField] private List<int> destroyThresholds;
    [SerializeField] private List<int> objectHitPoints;
    [SerializeField] private List<int> objectDestroyWait;
    
    
    public int StartScore => startScore;
    public List<int> LevelThresholds => levelThresholds; 
    public List<int> RewardPoints => rewardPoints;
    
    [Header("Score System")]
    [SerializeField] private int startScore;
    [SerializeField] private List<int> levelThresholds; //This has to begin with 0
    [SerializeField] private List<int> rewardPoints;


    public int MatchTimeAsSecond => matchTimeAsSeconds;
    
    [Header("Game Mode Setting")] 
    [SerializeField] private int matchTimeAsSeconds;

}
