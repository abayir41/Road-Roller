using System.Collections.Generic;

public class ScoreSystem
{
    //Level Rewarding
    private List<int> LevelThresholds { get; }
    private List<int> RewardPoints { get; }
    
    //Player and properties
    private Player Player { get; }

    private int CurrentLevel { get => Player.Level;
        set => Player.Level = value; }

    private int CurrentScore { get => Player.Score;
        set => Player.Score = value; }

    //Trigger
    private CarActionSys CarActionSys { get; }
    
    
    private bool _maxLevelReached;

    public ScoreSystem(CarActionSys carActionSys,List<int> levelThresholds,List<int> rewardPoints, Player player)
    {
        CarActionSys = carActionSys;
        RewardPoints = rewardPoints;
        LevelThresholds = levelThresholds;
        Player = player;
    }

    public float RatioOfBetweenLevels()
    {
        if (_maxLevelReached) return 0f;
        var diffBetweenLevel = LevelThresholds[CurrentLevel] - LevelThresholds[CurrentLevel - 1];
        var ourPoint = CurrentScore - LevelThresholds[CurrentLevel - 1];
        var result = (float) ourPoint / diffBetweenLevel;
        return result;
    }
    
    public void AddScore(int score)
    {
        CurrentScore += score;

        if (!_maxLevelReached)
        {
            while (CurrentScore >= LevelThresholds[CurrentLevel])
            {
                CarActionSys.LevelUpped(RewardPoints[CurrentLevel - 1]);
                CurrentLevel += 1;
                if (CurrentLevel == LevelThresholds.Count)
                {
                    _maxLevelReached = true;
                    CarActionSys.MaxLevelReached();
                    break;
                }
            }
        }
    }
}
