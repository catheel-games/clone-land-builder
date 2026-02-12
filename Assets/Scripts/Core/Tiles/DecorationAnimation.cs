using UnityEngine;
using DG.Tweening;

public class DecorationAnimation : MonoBehaviour
{
    [SerializeField] private float archHeight = 0.07f;
    [SerializeField] private float diveDepth = 0.12f;
    [SerializeField] private float archDuration = 1.3f;
    [SerializeField] private float underwaterTime = 4f;
    [SerializeField] private float maxTilt = 75f;

    void Start()
    {
        transform.localPosition = new Vector3(0, -diveDepth, 0);
        AnimateCycle();
    }

    private void AnimateCycle()
    {
        transform.localRotation = Quaternion.Euler(-maxTilt, 0, 0);

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOLocalMoveY(archHeight, archDuration * 0.5f).SetEase(Ease.OutSine));
        seq.Append(transform.DOLocalMoveY(-diveDepth, archDuration * 0.5f).SetEase(Ease.InSine));

        seq.Insert(0, transform.DOLocalRotate(new Vector3(maxTilt, 0, 0), archDuration).SetEase(Ease.InOutSine));

        seq.AppendCallback(() => transform.localRotation = Quaternion.Euler(0, 0, 0));
        seq.AppendInterval(underwaterTime);

        seq.OnComplete(() => AnimateCycle());
    }
}
