using UnityEngine;

public class Pausing : MonoBehaviour
{
    public void Pause()
    {
        InputManager.Instance.DisableInput();
    }

    public void UnPause()
    {
        if (GameData.Instance.ftueIsEnded)
        {
            InputManager.Instance.EnableInput();
        }
    }
}
