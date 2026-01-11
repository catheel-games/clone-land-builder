using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public struct Setting
    {
        public string preferenceName;
        public bool defaultValue;
        public SettingsButton button;
    }

    [SerializeField] private Setting[] settings;

    private bool[] settingValues = new bool[4];

    [SerializeField] SettingsButton[] settingsButton;

    // private bool MusicIsOn;
    // private bool SoundIsOn;
    // private bool VibrationIsOn;
    // private bool SixtyFPSIsOn;

    void Awake()
    {
        StartSettings();
    }

    public void StartSettings()
    {
        deriveSettingValues();

        SetMusic();
        SetSounds();
        SetVibration();
        SetFPS();
    }

    private void deriveSettingValues()
    {
        for (int i = 0; i < settings.Length; i++)
        {
            if (!PlayerPrefs.HasKey(settings[i].preferenceName))
            {
                PlayerPrefs.SetInt(settings[i].preferenceName, settings[i].defaultValue ? 1 : 0);
            }

            settingValues[i] = settings[i].defaultValue;
        }
    }

    public void ToggleMusic()
    {
        MusicIsOn = !MusicIsOn;
        SetMusic();
    }

    public void ToggleSounds()
    {
        SoundIsOn = !SoundIsOn;
        SetSounds();
    }

    public void ToggleVibration() {
        VibrationIsOn = !VibrationIsOn;
        SetVibration();
    }

    public void ToggleFPS()
    {
        SixtyFPSIsOn = !SixtyFPSIsOn;
        SetFPS();
    }

    private void SetMusic()
    {
        PlayerPrefs.SetInt("MusicSettings", MusicIsOn ? 1 : 0);
        settingsButton[0].SetButton(MusicIsOn);
        
        AudioManager.Instance.SetMusicEnabled(MusicIsOn);
    }

    private void SetSounds()
    {
        PlayerPrefs.SetInt("SoundSetting", SoundIsOn ? 1 : 0);
        settingsButton[1].SetButton(SoundIsOn);

        AudioManager.Instance.SetSoundEnabled(SoundIsOn);
    }

    private void SetVibration()
    {
        PlayerPrefs.SetInt("VibrationSetting", VibrationIsOn ? 1 : 0);
        settingsButton[2].SetButton(VibrationIsOn);

        VibrationManager.Instance.ChangeVibration(VibrationIsOn);
    }

    private void SetFPS()
    {
        PlayerPrefs.SetInt("FPSSetting", SixtyFPSIsOn ? 1 : 0);
        settingsButton[3].SetButton(SixtyFPSIsOn);

        Application.targetFrameRate = SixtyFPSIsOn ? 60 : 30;
    }
}
