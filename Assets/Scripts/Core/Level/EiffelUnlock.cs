using UnityEngine;
using DG.Tweening;

public class EiffelUnlock : MonoBehaviour
{
    [SerializeField] private LevelCanvasControl levelCanvasControl;
    [SerializeField] private LevelTileUnlocking levelTileUnlocking;
    [SerializeField] private FTUE _FTUE;

    [Header("Eiffel Icon Rotation")]
    [SerializeField] private RectTransform eiffelIconRect;
    [SerializeField] private float eiffelIconRotY;
    [SerializeField] private float eiffelIconRotDuration;

    private void Start()
    {
        DOVirtual.DelayedCall(1f, () => EiffelUnlocking());
    }

    public void EiffelUnlocking()
    {
        InputManager.Instance.DisableInput();

        Sequence eiffelSeq = DOTween.Sequence();

        eiffelSeq.Append(eiffelIconRect.DORotate(new Vector3(0, eiffelIconRotY, 0),
                        eiffelIconRotDuration, RotateMode.FastBeyond360)
                        .SetEase(Ease.InCubic))
                 .AppendCallback(() =>
                 {
                     Vector3 worldPos = eiffelIconRect.position;
                     eiffelIconRect.anchorMin = eiffelIconRect.anchorMax = new Vector2(0.5f, 0.5f);
                     eiffelIconRect.pivot = new Vector2(0.5f, 0.5f);
                     eiffelIconRect.position = worldPos;
                 })
                 .Append(eiffelIconRect.DOAnchorPos(Vector2.zero, 0.25f).SetEase(Ease.OutCubic))

                 .AppendCallback(() => levelTileUnlocking.SetEiffelTower())
                 .AppendCallback(() => levelCanvasControl._eiffelPopupContainer.Show())
                 .AppendInterval(0.3f)
                 .AppendCallback(() => eiffelIconRect.gameObject.SetActive(false));
    }

    public void StartTutorials()
    {
        _FTUE.EiffelUnlockingTutorial();
    }
}
