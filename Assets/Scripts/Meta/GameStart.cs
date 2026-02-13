using UnityEngine;

public class GameStart : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.PlayMusicGroup("Loading");

        if (GameData.Instance.currentLevelID == 0)
            LoadManager.Instance.LoadScene("Menu");

        else
            LoadManager.Instance.LoadScene("Game");
    }
}
