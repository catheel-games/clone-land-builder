using UnityEngine;

public class LevelInterfaceData : MonoBehaviour
{
    [SerializeField] private GameObject starCircleObject;
    [SerializeField] private GameObject starCounterObject;

    [SerializeField] private GameObject endLevelPanelObject;


    private void Start()
    {
        int currentLevel = GameData.Instance.currentLevelID;

        LevelProgress progress = GameData.Instance.GetLevelProgress(currentLevel);

        if (progress.state == LevelProgress.LevelState.Finished)
        {
            starCircleObject.SetActive(false);
            starCounterObject.SetActive(false);
            endLevelPanelObject.SetActive(true);
        }

        else
        {
            starCircleObject.SetActive(true);
            starCounterObject.SetActive(true);
            endLevelPanelObject.SetActive(false);
        }
    }
}
