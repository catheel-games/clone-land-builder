using DG.Tweening;
using UnityEngine;

public class UIDefault : UIContainer
{
    [SerializeField] private float fadeDuration = 0.3f;

    public override void Show()
    {
        Sequence sequence = DOTween.Sequence();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        sequence.Append(canvasGroup.DOFade(1f, fadeDuration));
        sequence.AppendCallback(() => base.Show());
    }

    public override void Hide()
    {
        Sequence sequence = DOTween.Sequence();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        sequence.Append(canvasGroup.DOFade(0f, fadeDuration));
        sequence.AppendCallback(() => base.Hide());
    }
}
