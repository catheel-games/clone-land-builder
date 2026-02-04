using UnityEngine;

public class GameStart : MonoBehaviour
{
    void Start()
    {
        if (GameData.Instance.currentLevelID == 0)
            LoadManager.Instance.LoadScene("Menu");

        else
            LoadManager.Instance.LoadScene("Game");
    }
}
