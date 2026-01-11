using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private UIContainer levelSelection;
    [SerializeField] private UIContainer completedLevelSelection;

    public void MenuSelectLevel(int level)
    {
        if (Random.Range(0, 2) == 0)
        {
            levelSelection.Show();
        }
        else
        {
            completedLevelSelection.Show();
        }
    }
}
