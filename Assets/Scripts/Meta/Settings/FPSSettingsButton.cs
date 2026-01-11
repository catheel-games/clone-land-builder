using UnityEngine;
using TMPro;

public class FPSSettingsButton : SettingsButton
{
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private TMP_Text fpsText;
    [SerializeField] private string onValue = "60";
    [SerializeField] private string offValue = "30";
    [SerializeField] private Color onColor = Color.white;
    [SerializeField] private Color offColor = Color.gray;

    protected override void ApplyVisual()
    {
        base.ApplyVisual();

        numberText.text = isActive ? onValue : offValue;
        numberText.color = isActive ? onColor : offColor;
        fpsText.color = isActive ? onColor : offColor;
    }

    protected override void ApplyAction()
    {
        GameManager.Instance.SetFPS(isActive ? 60 : 30);
    }
}
