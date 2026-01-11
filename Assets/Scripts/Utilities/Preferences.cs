using UnityEngine;

public static class Preferences
{
    public static bool GetBool(string preferenceKey, bool defaultValue)
    {
        if (!PlayerPrefs.HasKey(preferenceKey))
        {
            PlayerPrefs.SetInt(preferenceKey, defaultValue ? 1 : 0);
            return defaultValue;
        }
        else
        {
            return PlayerPrefs.GetInt(preferenceKey) == 1;
        }
    }

    public static void SetBool(string preferenceKey, bool value)
    {
        PlayerPrefs.SetInt(preferenceKey, value ? 1 : 0);
    }
}
