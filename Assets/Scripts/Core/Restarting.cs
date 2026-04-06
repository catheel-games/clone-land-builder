using UnityEngine;

public class Restarting : MonoBehaviour
{
    [SerializeField] private UIContainer restartPanelContainer;
    [SerializeField] private UIButton restartButton;
    [SerializeField] private UIButton backButton;

    void OnEnable()
    {
        restartButton.OnUp += ShowRestartPanel;
        backButton.OnUp += HideRestartPanel;
    }

    void OnDisable()
    {
        restartButton.OnUp -= ShowRestartPanel;
        backButton.OnUp -= HideRestartPanel;
    }

    private void ShowRestartPanel()
    {
        restartPanelContainer.Show();
    }

    private void HideRestartPanel()
    {
        restartPanelContainer.Hide();
    }
}
