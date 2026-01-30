using DG.Tweening;
using TMPro;
using UnityEngine;

public class StarScoreBonusBlinkFeedback : MonoBehaviour
{
    [SerializeField] private float blinkDuration = 1.0f;
    [SerializeField] private TextMeshProUGUI bonusText;

    void Start()
    {
        // should be called in a higher unit
        // alas for now it goes like this
        Play();
    }

    public void Play()
    {
        Sequence loopedBlinkingSequence = DOTween.Sequence();

        loopedBlinkingSequence.Append(bonusText.DOFade(0, blinkDuration * 0.5f));
        loopedBlinkingSequence.Append(bonusText.DOFade(1, blinkDuration * 0.5f));
        loopedBlinkingSequence.SetLoops(-1, LoopType.Restart);
    }
}
