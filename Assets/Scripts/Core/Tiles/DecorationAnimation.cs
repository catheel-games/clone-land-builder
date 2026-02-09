using UnityEngine;
using DG.Tweening;

public class DecorationAnimation : MonoBehaviour
{
    [SerializeField] private float archHeight = 0.07f;
    [SerializeField] private float diveDepth = 0.12f;
    [SerializeField] private float archDuration = 1.3f;
    [SerializeField] private float underwaterTime = 4f;
    [SerializeField] private float maxTilt = 75f;
    [SerializeField] private float arcLength = 0.3f;

    void Start()
    {
        AnimateCycle();
    }

    private void AnimateCycle()
    {
        transform.localPosition = new Vector3(0, -diveDepth, -arcLength);
        transform.localRotation = Quaternion.Euler(-maxTilt, 0, 0);

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOLocalMoveY(archHeight, archDuration * 0.5f).SetEase(Ease.OutSine));
        seq.Join(transform.DOLocalMoveZ(0, archDuration * 0.5f).SetEase(Ease.OutSine));

        seq.Append(transform.DOLocalMoveY(-diveDepth, archDuration * 0.5f).SetEase(Ease.InSine));
        seq.Join(transform.DOLocalMoveZ(arcLength, archDuration * 0.5f).SetEase(Ease.InSine));

        seq.Insert(0, transform.DOLocalRotate(new Vector3(maxTilt, 0, 0), archDuration).SetEase(Ease.InOutSine));

        seq.AppendInterval(underwaterTime);

        seq.OnComplete(() => AnimateCycle());
    }
}
