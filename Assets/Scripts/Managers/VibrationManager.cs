using UnityEngine;

public class VibrationManager : Singleton<VibrationManager>
{
    [SerializeField] private bool vibrationEnabled = true;

    void Start()
    {
        Vibration.Init();
    }

    public void SetVibration(bool isEnabled)
    {
        vibrationEnabled = isEnabled;
    }

    public void Vibrate()
    {
        #if UNITY_ANDROID
        if (vibrationEnabled)
        {
            Debug.Log("Vibrate");
            Vibration.VibrateAndroid(20);
        }
        #endif
    }
}