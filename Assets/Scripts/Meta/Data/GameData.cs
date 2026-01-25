using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public List<LevelProgress> levelsProgress = new();
    public int currentLevelID = 1;

    private static GameData _instance = null;
    public static GameData Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameData();
            return _instance;
        }
    }

    public LevelProgress GetLevelProgress(int levelIndex)
    {
        return levelsProgress.Find(l => l.levelIndex == levelIndex);
    }

    public void SetData(List<LevelProgress> levels)
    {
        for (int i = 0; i < levelsProgress.Count; i++)
        {
            levelsProgress[i] = levels[i];
        }
    }

}


[System.Serializable]
public class LevelProgress
{
    public int levelIndex;
    public int currentScore;
    public int currentTilesLeft;
    public LevelState state;

    public enum LevelState
    {
        Blocked,
        Unlocked,
        InProgress,
        Finished
    }
}
