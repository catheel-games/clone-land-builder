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

    public void SetUnlockedObject()
    {
        tapToContinueCanvasGroup.DOFade(0, 0);

        int currentLevel = GameData.Instance.currentLevelID;

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
}
