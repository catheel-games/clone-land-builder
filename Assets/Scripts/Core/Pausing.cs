using UnityEngine;

public class Pausing : MonoBehaviour
{
    public void Pause()
    {
        InputManager.Instance.DisableInput();
    }

    public void UnPause()
    {
        if (SaveLoadManager.Instance.gameData.ftueIsEnded)
        {
            InputManager.Instance.EnableInput();
        }
    }
}
