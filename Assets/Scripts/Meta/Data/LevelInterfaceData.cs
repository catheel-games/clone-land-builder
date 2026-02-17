using UnityEngine;

public class LevelInterfaceData : MonoBehaviour
{
    [SerializeField] private GameObject starCircleObject;
    [SerializeField] private GameObject starCounterObject;

    [SerializeField] private GameObject endLevelPanelObject;
    [SerializeField] private GameObject coinSection;
    [SerializeField] private GameObject eiffelObject;


    private void Start()
    {
        int currentLevel = GameData.Instance.currentLevelID;

        LevelProgress progress = GameData.Instance.GetLevelProgress(currentLevel);

        if (progress.state == LevelProgress.LevelState.Finished)
        {
            starCircleObject.SetActive(false);
            starCounterObject.SetActive(false);
            endLevelPanelObject.SetActive(true);
            coinSection.SetActive(true);
        }

        else
        {
            starCircleObject.SetActive(true);
            starCounterObject.SetActive(true);
            endLevelPanelObject.SetActive(false);

            if (currentLevel == 1)
                coinSection.SetActive(false);
            else
                coinSection.SetActive(true);
        }

        if (currentLevel == 2)
        {
            if (!GameData.Instance.eiffelIsUnlocked)
            {
                eiffelObject.SetActive(true);
            }
        }
    }
}
