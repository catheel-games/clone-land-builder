using UnityEngine;
using TMPro;
using System.Collections;

public class SettingsController : MonoBehaviour
{
    [SerializeField] SettingsButton[] settingsButton;
    [SerializeField] private float cooldown = 0.15f;

    private bool MusicIsOn;
    private bool SoundIsOn;
    private bool VibrationIsOn;
    private bool SixtyFPSIsOn;

    private bool canToggle = true;

    void Awake()
    {
        StartSettings();
    }

    public void StartSettings()
    {
        if (!PlayerPrefs.HasKey("MusicSettings")) PlayerPrefs.SetInt("MusicSettings", 1);
        if (!PlayerPrefs.HasKey("SoundSetting")) PlayerPrefs.SetInt("SoundSetting", 1);
        if (!PlayerPrefs.HasKey("VibrationSetting")) PlayerPrefs.SetInt("VibrationSetting", 1);
        if (!PlayerPrefs.HasKey("FPSSetting")) PlayerPrefs.SetInt("FPSSetting", 1);

        MusicIsOn = PlayerPrefs.GetInt("MusicSettings") == 1;
        SoundIsOn = PlayerPrefs.GetInt("SoundSetting") == 1;
        VibrationIsOn = PlayerPrefs.GetInt("VibrationSetting") == 1;
        SixtyFPSIsOn = PlayerPrefs.GetInt("FPSSetting") == 1;

        SetMusic();
        SetSounds();
        SetVibration();
        SetFPS();
    }

    public void ToggleMusic()
    {
        if (canToggle)
        {
            StartCoroutine(CooldownTimer());
            MusicIsOn = !MusicIsOn;

            SetMusic();
        }
    }

    public void ToggleSounds()
    {
        if (canToggle)
        {
            StartCoroutine(CooldownTimer());
            SoundIsOn = !SoundIsOn;

            SetSounds();
        }
    }

    public void ToggleVibration() {
        if (canToggle)
        {
            StartCoroutine(CooldownTimer());
            VibrationIsOn = !VibrationIsOn;

            SetVibration();
        }
    }

    public void ToggleFPS()
    {
        if (canToggle)
        {
            StartCoroutine(CooldownTimer());
            SixtyFPSIsOn = !SixtyFPSIsOn;
            SetFPS();
        }
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

    IEnumerator CooldownTimer()
    {
        canToggle = false;
        yield return new WaitForSeconds(cooldown);
        canToggle = true;
    }
}
