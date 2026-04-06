using UnityEngine;

public class Pausing : MonoBehaviour
{
    [SerializeField] private UIContainer pausePanelContainer;
    [SerializeField] private UIButton continueButton;

    void OnEnable()
    {
        continueButton.OnUp += Pause;
    }

    void OnDisable()
    {
        continueButton.OnUp -= Pause;
    }

    public void Pause()
    {
        InputManager.Instance.DisableInput();
    }

    public void UnPause()
    {
        pausePanelContainer.Hide();
        
        if (SaveLoadManager.Instance.gameData.progressData.ftueIsEnded)
        {
            InputManager.Instance.EnableInput();
        }
    }
}
