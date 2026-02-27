using UnityEngine;
using TMPro;
public class LevelInterfaceData : MonoBehaviour
{
    [SerializeField] private GameObject starCircleObject;
    [SerializeField] private GameObject starCounterObject;

    [SerializeField] private GameObject endLevelPanelObject;
    [SerializeField] private GameObject coinSection;
    [SerializeField] private GameObject eiffelObject;

    [SerializeField] private TextMeshProUGUI levelIDText;


    private void Start()
    {
        int currentLevel = SaveLoadManager.Instance.gameData.progressData.currentLevelID;

        LevelProgress progress = SaveLoadManager.Instance.gameData.GetLevelProgress(currentLevel);

        levelIDText.text = "Level " + currentLevel;

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

        eiffelObject.SetActive(false);

        if (currentLevel == 2)
        {
            if (!SaveLoadManager.Instance.gameData.progressData.eiffelIsUnlocked)
            {
                eiffelObject.SetActive(true);
            }
        }
    }
}
