using DG.Tweening;
using UnityEngine;

public class TilePreviewStarScaleInFeedback : MonoBehaviour
{
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private float maxScale = 1.8f;

    Sequence scaleInSequence;

    public void Play()
    {
        scaleInSequence = DOTween.Sequence();

        transform.localScale = Vector3.zero;

        scaleInSequence.Append(transform.DOScale(maxScale, duration * 0.8f));
        scaleInSequence.Append(transform.DOScale(Vector3.one, duration * 0.2f));
    }

    public void Kill()
    {
        scaleInSequence.Kill();
    }
}
