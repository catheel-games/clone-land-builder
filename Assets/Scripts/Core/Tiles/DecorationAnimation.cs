using UnityEngine;
using DG.Tweening;

public class DecorationAnimation : MonoBehaviour
{
    [SerializeField] private float archHeight = 0.15f;
    [SerializeField] private float diveDepth = 0.1f;
    [SerializeField] private float archDuration = 2.5f;
    [SerializeField] private float underwaterTime = 5f;
    [SerializeField] private float maxTilt = 25f;

    void Start()
    {
        AnimateCycle();
    }

    private void AnimateCycle()
    {
        transform.localPosition = new Vector3(0, -diveDepth, 0);
        transform.localRotation = Quaternion.Euler(-maxTilt, 0, 0);

        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOLocalMoveY(archHeight, archDuration * 0.5f).SetEase(Ease.OutSine));
        seq.Append(transform.DOLocalMoveY(-diveDepth, archDuration * 0.5f).SetEase(Ease.InSine));

        seq.Insert(0, transform.DOLocalRotate(new Vector3(maxTilt, 0, 0), archDuration).SetEase(Ease.Linear));

        seq.AppendInterval(underwaterTime);

        seq.OnComplete(() => AnimateCycle());
    }
}
