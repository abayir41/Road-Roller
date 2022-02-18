using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    public string HouseTag => houseTag;
    [SerializeField] private string houseTag;

    public string DozerTag => dozerTag;
    [SerializeField] private string dozerTag;
    
    public string BaseSkinID => DozerSkins[0].ItemID;
    
    
    
    public string TotalScore => totalScore;
    public string SelectedSkinIndexString => selectedSkinIndexString;
    public string SavedProgressSkinUnlock => savedProgressSkinUnlock;
    public string UnlockedSkinString => unlockedSkinString;
    public string ScoreSaverText => scoreSaverText;
    [Header("Just Be Sure there are not any same string")]
    [SerializeField] private string scoreSaverText;
    [SerializeField] private string totalScore;
    [SerializeField] private string selectedSkinIndexString;
    [SerializeField] private string savedProgressSkinUnlock;
    [SerializeField] private string unlockedSkinString;

    
    
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


    public int MatchTimeAsSecond => matchTimeAsSeconds;
    
    [Header("Game Mode Setting")] 
    [SerializeField] private int matchTimeAsSeconds;
    
    
    public List<SkinScriptable> DozerSkins => dozerSkins;

    [Header("Skin System")]
    [SerializeField] private List<SkinScriptable> dozerSkins;


    public List<string> Names => names;
    public List<Color> Colors => colors;
    
    [Header("Leaderboard System")]
    [SerializeField] private List<string> names;
    [SerializeField] private List<Color> colors;

}
