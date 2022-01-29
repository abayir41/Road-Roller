using System.Collections.Generic;

public class ScoreSystem
{
    public int CurrentLevel { get; private set; } = 1;

    public int CurrentScore { get; private set; }

    private readonly List<int> _levelThresholds;
    private readonly List<int> _rewardPoints;
    private bool _maxLevelReached;
    private readonly CarActionSys _carActionSys;
    private readonly LeaderBoardSystem _leaderBoardSystem;
    private readonly string _playerName; 
    
    public ScoreSystem(CarActionSys carActionSys,List<int> levelThresholds,List<int> rewardPoints,int startScore,LeaderBoardSystem leaderBoardSystem,string playerName)
    {
        CurrentScore = startScore;
        _carActionSys = carActionSys;
        _rewardPoints = rewardPoints;
        _levelThresholds = levelThresholds;
        _leaderBoardSystem = leaderBoardSystem;
        _playerName = playerName;
        _leaderBoardSystem.AddScore(_playerName,startScore);
    }

    public float RatioOfBetweenLevels()
    {
        if (_maxLevelReached) return 0f;
        var diffBetweenLevel = _levelThresholds[CurrentLevel] - _levelThresholds[CurrentLevel - 1];
        var ourPoint = CurrentScore - _levelThresholds[CurrentLevel - 1];
        var result = (float) ourPoint / diffBetweenLevel;
        return result;
    }
    public void AddScore(int score)
    {
        CurrentScore += score;

        if (!_maxLevelReached)
        {
            while (CurrentScore >= _levelThresholds[CurrentLevel])
            {
                _carActionSys.LevelUpped(_rewardPoints[CurrentLevel - 1]);
                CurrentLevel += 1;
                if (CurrentLevel == _levelThresholds.Count)
                {
                    _maxLevelReached = true;
                    _carActionSys.MaxLevelReached();
                    break;
                }
            }
        }
        
        _leaderBoardSystem.SetScore(_playerName,CurrentScore);
    }
}
