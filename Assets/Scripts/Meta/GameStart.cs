using UnityEngine;

public class GameStart : MonoBehaviour
{
    void Start()
    {
        LoadManager.Instance.LoadScene("Menu");
    }
}
