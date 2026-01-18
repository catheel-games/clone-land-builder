using UnityEngine;

public static class Preferences
{
    public static bool GetBool(string preferenceKey, bool defaultValue)
    {
        if (PlayerPrefs.HasKey(preferenceKey))
        {
            return PlayerPrefs.GetInt(preferenceKey) == 1;
        }
        else
        {
            PlayerPrefs.SetInt(preferenceKey, defaultValue ? 1 : 0);
            return defaultValue;
        }
    }

    public static void SetBool(string preferenceKey, bool value)
    {
        PlayerPrefs.SetInt(preferenceKey, value ? 1 : 0);
    }
}
