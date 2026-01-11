using UnityEngine;

public class MusicSettingsButton : SettingsButton
{
    protected override void ApplyAction()
    {
        AudioManager.Instance.SetMusicVolume(isActive);
    }
}
