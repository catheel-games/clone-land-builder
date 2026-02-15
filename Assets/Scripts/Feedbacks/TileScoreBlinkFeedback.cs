using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TileScoreBlinkFeedback : MonoBehaviour
{
    private bool changedColor = false;

    [SerializeField] private Image counterImage;
    [SerializeField] private TextMeshProUGUI counterText;

    [SerializeField] private float blinkDuration = 1f;

    [Header("Colors")]
    [SerializeField] private Color counterDefaultColor;
    [SerializeField] private Color counterLowColor;

    [SerializeField] private Color counterTextDefaultColor;
    [SerializeField] private Color counterTextLowColor;

    public void ChangeCounterLowColor()
    {
        if (!changedColor)
        {
            changedColor = true;

            counterText.color = counterTextLowColor;
            counterImage.DOColor(counterLowColor, 0f);
        }

        StartBlinking();
    }

    public void ChangeCounterDefaultColor()
    {
        if (changedColor)
        {
            changedColor = false;

            counterText.color = counterTextDefaultColor;
            counterImage.DOColor(counterDefaultColor, 0.1f);

            //loopedBlinkingSequence.Kill();
        }
    }

    public void StartBlinking()
    {
        Sequence loopedBlinkingSequence = DOTween.Sequence();

        loopedBlinkingSequence.Append(counterImage.DOFade(0.5f, blinkDuration * 0.5f));
        loopedBlinkingSequence.Append(counterImage.DOFade(1, blinkDuration * 0.5f));
        loopedBlinkingSequence.SetLoops(3, LoopType.Restart);
    }
}
