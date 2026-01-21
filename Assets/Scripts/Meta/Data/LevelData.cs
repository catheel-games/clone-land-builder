using UnityEngine;

public class LevelData : MonoBehaviour
{
    private LevelDataSO levelData;

    private LevelProgress progress;

    void Start()
    {
        levelData = SaveLoadManager.Instance.levelDatas[GameData.Instance.currentLevelID-1];

        progress = GameData.Instance.GetLevelProgress(levelData.levelIndex);

        if (progress == null)
        {
            progress = new LevelProgress
            {
                levelIndex = levelData.levelIndex,
                currentScore = 0,
                currentTilesLeft = 50,
                state = LevelProgress.LevelState.Unlocked
            };

            GameData.Instance.levelsProgress.Add(progress);
        }
    }   
}
