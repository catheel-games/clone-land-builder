using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour
{
    [SerializeField] private LevelSelection levelSelection;
    [SerializeField] private Button button;
    [SerializeField] private int levelIndex;

    void Awake()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        levelSelection.MenuSelectLevel(levelIndex);
        VibrationManager.Instance.Vibrate();
    }
}
