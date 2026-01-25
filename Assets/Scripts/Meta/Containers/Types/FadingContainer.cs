using DG.Tweening;
using UnityEngine;

public class FadingContainer : UIContainer
{
    [SerializeField] private float fadeDuration = 0.3f;

    public override void Show()
    {
        if (!isActive)
        {
            Sequence sequence = DOTween.Sequence();

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            AudioManager.Instance.PlaySound("UI", "Open UI");

            sequence.Append(canvasGroup.DOFade(1f, fadeDuration));
            sequence.AppendCallback(() => base.Show());
        }
    }

    public override void Hide()
    {
        if (isActive)
        {
            Sequence sequence = DOTween.Sequence();

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            AudioManager.Instance.PlaySound("UI", "Close UI");

            sequence.Append(canvasGroup.DOFade(0f, fadeDuration));
            sequence.AppendCallback(() => base.Hide());
        }
    }
}
