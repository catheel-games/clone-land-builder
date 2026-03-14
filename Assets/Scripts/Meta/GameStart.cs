using UnityEngine;

public class GameStart : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayMusicGroup("Loading");

        if (SaveLoadManager.Instance.gameData.progressData.currentLevelID == 0)
            LoadManager.Instance.LoadScene("Menu");

        else
            LoadManager.Instance.LoadScene("Game");
    }
}
