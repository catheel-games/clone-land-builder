using UnityEngine;

public class LevelDataSetup : MonoBehaviour
{
    [SerializeField] private StarCollecting starCollecting;

    private LevelProgress progress;
    private int currentLevel;

    void Start()
    {
        SetData();
    }


    public void SetData()
    {
        currentLevel = SaveLoadManager.Instance.gameData.progressData.currentLevelID;
        LevelDataSO levelData = SaveLoadManager.Instance.levelDatas[currentLevel-1];

        progress = SaveLoadManager.Instance.gameData.GetLevelProgress(currentLevel);



        LevelControl.Instance.tileScore = progress.currentTilesLeft;
        LevelControl.Instance.starScore = progress.currentScore;

        starCollecting.SetDataUI(levelData.goalValue);
    }

    public void SaveData()
    {
        if (progress.state == LevelProgress.LevelState.Unlocked) {
            progress.state = LevelProgress.LevelState.InProgress;
        }

        SaveLoadManager.Instance.gameData.levelsProgress[currentLevel - 1].currentScore = LevelControl.Instance.starScore;
        SaveLoadManager.Instance.gameData.levelsProgress[currentLevel - 1].currentTilesLeft = LevelControl.Instance.tileScore;

        SaveLoadManager.Instance.SaveData();
    }
}
