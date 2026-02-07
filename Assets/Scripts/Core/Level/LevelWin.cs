using UnityEngine;

public class LevelWin : MonoBehaviour
{
    public void Winning()
    {
        SettingData();
    }

    private void SettingData()
    {
        int currentLevel = GameData.Instance.currentLevelID;

        LevelProgress currentLevelProgress = GameData.Instance.GetLevelProgress(currentLevel);
        LevelProgress nextLevelProgress = GameData.Instance.GetLevelProgress(currentLevel + 1);

        currentLevelProgress.state = LevelProgress.LevelState.Finished;

        if (nextLevelProgress != null)
            nextLevelProgress.state = LevelProgress.LevelState.Unlocked;

        SaveLoadManager.Instance.SaveData();
    }
}
