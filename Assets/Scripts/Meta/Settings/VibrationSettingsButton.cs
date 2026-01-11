using UnityEngine;

public class VibrationSettingsButton : SettingsButton
{
    protected override void ApplyAction()
    {
        VibrationManager.Instance.SetVibration(isActive);
    }
}