using UnityEngine;
using DG.Tweening;

public class CommentContainer : UIContainer
{
    [SerializeField] private float fadeDuration = 0.125f;
    private bool isVisible = false;

    public override void Show()
    {
        if (!isActive)
        {
            Sequence sequence = DOTween.Sequence();

            isVisible = true;

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            sequence.Join(transform.DOScale(1f, fadeDuration));
            sequence.Append(canvasGroup.DOFade(1f, fadeDuration));
            sequence.AppendCallback(() => base.Show());
        }
    }

    public override void Hide()
    {
        if (isActive)
        {
            Sequence sequence = DOTween.Sequence();

            isVisible = false;

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            sequence.Join(transform.DOScale(0f, fadeDuration));
            sequence.Append(canvasGroup.DOFade(0f, fadeDuration));
            sequence.AppendCallback(() => base.Hide());
        }
    }

    public void ToggleVisibility()
    {
        if (isVisible)
            Hide();
        else if (!isVisible)
            Show();
    }
}
