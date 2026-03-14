using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<LevelProgress> levelsProgress = new();
    public ProgressData progressData;

    public LevelProgress GetLevelProgress(int levelIndex)
    {
        return levelsProgress.Find(l => l.levelIndex == levelIndex);
    }

    public void SetData(List<LevelProgress> levels, ProgressData progress)
    {
        levelsProgress = new List<LevelProgress>(levels);
        progressData = progress;
    }

}

[System.Serializable]
public class ProgressData
{
    public int currentLevelID = 1;
    public int coin = 0;
    public int eiffelScore = 0;
    public bool ftueIsEnded = false;
    public bool matched3Tiles = false;
    public bool eiffelIsUnlocked = false;
    public bool blueSphereTutored = false;
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
