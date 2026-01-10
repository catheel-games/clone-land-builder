using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    [SerializeField] private TMP_Text numberText;
    [SerializeField] private TMP_Text fpsText;
    [SerializeField] private string onValue = "60";
    [SerializeField] private string offValue = "30";
    [SerializeField] private Color onColor = Color.white;
    [SerializeField] private Color offColor = Color.gray;

    public void SetButton(bool isOn)
    {
        buttonImage.sprite = isOn ? onSprite : offSprite;

        if (numberText != null)
        {
            numberText.text = isOn ? onValue : offValue;
            numberText.color = isOn ? onColor : offColor;
        }

        if (fpsText != null)
        {
            fpsText.color = isOn ? onColor : offColor;
        }
    }
}