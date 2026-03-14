using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private UIContainer levelSelection;
    [SerializeField] private UIContainer completedLevelSelection;

    [SerializeField] private LevelSelectionPanel levelSelectionPanel;
    [SerializeField] private LevelSelectionPanel completedLevelSelectionPanel;

    [SerializeField] private Animator[] levelAnimators;
    [SerializeField] private Image[] levelImages;
    [SerializeField] private TextMeshProUGUI[] levelNumberTexts;

    [SerializeField] private GameObject[] levelSimpleRoads;
    [SerializeField] private GameObject[] levelDottedRoads;
    [SerializeField] private GameObject[] levelInProgressObjects;

    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite greySprite;
    [SerializeField] private Color whiteColor;
    [SerializeField] private Color greenColor;


    private LevelDataSO levelData;
    private LevelProgress progress;

    private void Start()
    {
        levelSelectionPanel = levelSelection.gameObject.GetComponent<LevelSelectionPanel>();
        completedLevelSelectionPanel = completedLevelSelection.gameObject.GetComponent<LevelSelectionPanel>();

        DOVirtual.DelayedCall(0.02f, ()=>
        {
            SetLevelButtons();
        });
    }

    public void SetLevelButtons()
    {
        for (int i = 0; i < levelImages.Length; i++)
        {
            LevelProgress progress = SaveLoadManager.Instance.gameData.GetLevelProgress(i+1);

            LevelProgress.LevelState _state = progress != null ? progress.state : LevelProgress.LevelState.Blocked;

            levelInProgressObjects[i].SetActive(false);

            if (i != 0)
            {
                levelSimpleRoads[i-1].SetActive(true);
                levelDottedRoads[i-1].SetActive(false);
            }

            switch (_state) {
                case LevelProgress.LevelState.Blocked:
                    levelImages[i].sprite = greySprite;
                    levelAnimators[i].enabled = false;
                    levelNumberTexts[i].color = whiteColor;
                    if (i != 0) {
                        levelSimpleRoads[i-1].SetActive(false);
                        levelDottedRoads[i-1].SetActive(true);
                    }
                    break;

                case LevelProgress.LevelState.Unlocked:
                    levelImages[i].sprite = blueSprite;
                    levelAnimators[i].enabled = true;
                    levelNumberTexts[i].color = whiteColor;
                    break;
                case LevelProgress.LevelState.InProgress:
                    levelImages[i].sprite = blueSprite;
                    levelAnimators[i].enabled = true;
                    levelNumberTexts[i].color = whiteColor;
                    levelInProgressObjects[i].SetActive(true);
                    break;
                case LevelProgress.LevelState.Finished:
                    levelImages[i].sprite = greenSprite;
                    levelAnimators[i].enabled = false;
                    levelNumberTexts[i].color = greenColor;
                    break;
            }

        }
    }

    public void MenuSelectLevel(int level)
    {
        levelData = SaveLoadManager.Instance.levelDatas[level];

        progress = SaveLoadManager.Instance.gameData.GetLevelProgress(levelData.levelIndex);

        if (progress.state == LevelProgress.LevelState.Finished)
        {
            completedLevelSelection.Show();

            completedLevelSelectionPanel.SetResourceValues(levelData.levelTiles);
            completedLevelSelectionPanel.SetLevelIDText(level+1);
        }
        else if (progress.state == LevelProgress.LevelState.Blocked)
        {
            Debug.Log("This level is Blocked");
        }
        else
        {
            levelSelection.Show();

            bool played = progress.state == LevelProgress.LevelState.InProgress;
            levelSelectionPanel.SetValues(levelData.goalValue, levelData.coinValue, played, levelData.levelTiles);
            levelSelectionPanel.SetLevelIDText(level+1);
        }

        SaveLoadManager.Instance.gameData.progressData.currentLevelID = levelData.levelIndex;
        SaveLoadManager.Instance.SaveData();
    }
}
