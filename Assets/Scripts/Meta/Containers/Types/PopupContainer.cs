using UnityEngine;
using DG.Tweening;

public class PopupContainer : FadingContainer
{
    [SerializeField] private UIContainer topBar;

    [SerializeField] private Transform scalableTransform;
    [SerializeField] private float showScalingDuration = 0.25f;
    [SerializeField] private float hideScalingDuration = 0.4f;

    public override void Show()
    {
        base.Show();
        topBar.Hide();

        Sequence sequence = DOTween.Sequence();
        if (scalableTransform != null)
        {
            sequence.Append(scalableTransform.DOScale(0, 0));
            sequence.Append(scalableTransform.DOScale(1.1f, showScalingDuration));
            sequence.Append(scalableTransform.DOScale(1f, 0.05f));
        }

    }

    public override void Hide()
    {
        base.Hide();
        topBar.Show();

        Sequence sequence = DOTween.Sequence();

        if (scalableTransform != null)
        {
            sequence.Append(scalableTransform.DOScale(1.1f, 0.1f));
            sequence.Append(scalableTransform.DOScale(0f, hideScalingDuration));
        }
    }
}
