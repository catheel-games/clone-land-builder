using UnityEngine;

public class SoundSettingsButton : SettingsButton
{
    protected override void ApplyAction()
    {
        AudioManager.Instance.SetSoundVolume(isActive);
    }
}
