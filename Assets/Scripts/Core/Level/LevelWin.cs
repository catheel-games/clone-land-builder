using UnityEngine;

public class LevelWin : MonoBehaviour
{
    public void Winning()
    {
        SettingData();
    }

    private void SettingData()
    {
        int currentLevel = SaveLoadManager.Instance.gameData.currentLevelID;

        LevelProgress currentLevelProgress = SaveLoadManager.Instance.gameData.GetLevelProgress(currentLevel);
        LevelProgress nextLevelProgress = SaveLoadManager.Instance.gameData.GetLevelProgress(currentLevel + 1);

        currentLevelProgress.state = LevelProgress.LevelState.Finished;

        if (nextLevelProgress != null)
            nextLevelProgress.state = LevelProgress.LevelState.Unlocked;

        SaveLoadManager.Instance.SaveData();
    }
}
