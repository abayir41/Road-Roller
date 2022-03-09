using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MapConfig", menuName = "MapConfig")]
public class MapConfig : ScriptableObject
{
    public List<string> CollisionObjectFilter => collisionObjectFilter;
    public List<int> DestroyThresholdsFromLevels => destroyThresholdsFromLevelBase;
    public List<int> ObjectHitPoints => objectHitPoints;
    public List<int> ObjectDestroyWait => objectDestroyWait;
    
    [Header("Collision System")]
    [SerializeField] private List<string> collisionObjectFilter;
    [SerializeField] private List<int> destroyThresholdsFromLevelBase;
    [SerializeField] private List<int> objectHitPoints;
    [SerializeField] private List<int> objectDestroyWait;
    
    
    public int StartScore => startScore;
    public List<int> LevelThresholds => levelThresholds; 
    public List<int> RewardPoints => rewardPoints;
    
    [Header("Score System")]
    [SerializeField] private int startScore;
    [SerializeField] private List<int> levelThresholds; //This has to begin with 0
    [SerializeField] private List<int> rewardPoints;
    

    public int CameraDistanceDivider => cameraDistanceDivider;
    public float CameraSmoothness => cameraSmoothness;
    
    [Header("CamerasSettings")]
    [SerializeField] private float cameraSmoothness;
    [SerializeField] private int cameraDistanceDivider;


    public GameObject PlayerDozer => playerDozer;
    public GameObject AIDozer => aiDozer;
    
    [Header("Dozer Settings")]
    [SerializeField] private GameObject playerDozer;
    [SerializeField] private GameObject aiDozer;


    public float MapStartVelocity => mapStartVelocity;
    public float VelocityDivider => velocityDivider;
    public int PlayerCount => playerCount;
    
    [Header("Map Settings")] 
    [SerializeField] private float mapStartVelocity;
    [SerializeField] private float velocityDivider;
    [SerializeField] private int playerCount;


    public List<Sprite> NewDestroyableObjectImages => newDestroyableObjectImages;
    public Sprite NormalImage => normalImage;
    
    [Header("Next Level Images")]
    [SerializeField] private List<Sprite> newDestroyableObjectImages;
    [SerializeField] private Sprite normalImage;
}
