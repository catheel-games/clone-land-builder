using UnityEngine;
using DG.Tweening;
using TMPro;

public class LevelTileUnlocking : MonoBehaviour
{
    [SerializeField] private LevelCanvasControl levelCanvasControl;

    [SerializeField] private TextMeshProUGUI tileTypeText;
    [SerializeField] private CanvasGroup tapToContinueCanvasGroup;

    [SerializeField] private GameObject[] unlockedObjects;
    [SerializeField] private GameObject[] unlockedIconObjects;
    [SerializeField] private GameObject eiffelTowerObject;
    [SerializeField] private Camera tileUnlockedCamera;

    public void SetUnlockedObject()
    {
        tileUnlockedCamera.orthographicSize = 1f;
        tapToContinueCanvasGroup.DOFade(0, 0);

        int currentLevel = SaveLoadManager.Instance.gameData.progressData.currentLevelID;

        switch (currentLevel) {
            case 1:
                tileTypeText.text = "Forest";
                break;
            case 2:
                tileTypeText.text = "Field";
                break;
            case 3:
                tileTypeText.text = "Eternity";
                break;
        }

        eiffelTowerObject.SetActive(false);
        for (int i = 0; i < unlockedObjects.Length; i++) {
            unlockedObjects[i].SetActive(false);
            unlockedIconObjects[i].SetActive(false);
        }

        unlockedObjects[currentLevel - 1].SetActive(true);
        unlockedIconObjects[currentLevel - 1].SetActive(true);

        DOVirtual.DelayedCall(1.5f, () => {
            tapToContinueCanvasGroup.DOFade(1, 0.2f);
        });
    }

    public void ContinueToNextLevelPanel()
    {
        levelCanvasControl.ContinueToNextLevelPanel();
    }

    public void SetEiffelTower()
    {
        for (int i = 0; i < unlockedObjects.Length; i++)
        {
            unlockedObjects[i].SetActive(false);
        }

        tileUnlockedCamera.orthographicSize = 1.25f;

        eiffelTowerObject.transform.DOScale(0, 0);
        eiffelTowerObject.SetActive(true);
        eiffelTowerObject.transform.DOScale(1, 0.5f);

    }
}
