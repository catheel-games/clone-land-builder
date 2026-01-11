using UnityEngine;
using UnityEngine.UI;

public abstract class SettingsButton : MonoBehaviour
{
    [SerializeField] private string preferenceKey;
    [SerializeField] private bool defaultValue = true;

    [Header("Visuals")]
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    protected bool isActive;
    
    void Start()
    {
        isActive = Preferences.GetBool(preferenceKey, defaultValue);
        ApplyVisual();
        ApplyAction();
    }

    public void Toggle()
    {
        isActive = !isActive;
        Preferences.SetBool(preferenceKey, isActive);
        ApplyVisual();
        ApplyAction();
    }

    protected virtual void ApplyVisual()
    {
        buttonImage.sprite = isActive ? onSprite : offSprite;
    }

    protected abstract void ApplyAction();
}