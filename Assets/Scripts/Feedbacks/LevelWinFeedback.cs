using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LevelWinFeedback : MonoBehaviour
{
    [SerializeField] private LevelCanvasControl levelCanvasControl;

    [SerializeField] private Image starCircle;
    [SerializeField] private RectTransform starCounter;
    [SerializeField] private Image starCounterImage;
    [SerializeField] private GameObject starScoreCanvasObject;
    [SerializeField] private CanvasGroup starScoreCanvasGroup;
    [SerializeField] private RectTransform tickObject;

    [SerializeField] private Sprite newStarCircleSprite;
    [SerializeField] private Sprite newStarCounterSprite;

    private float starCounterTargetWidth = 85f;

    public void LevelWinning()
    {
        Sequence winSeq = DOTween.Sequence();

        winSeq.AppendCallback(levelCanvasControl.DisableWinContainers)
              .Join(starScoreCanvasGroup.DOFade(0, 0.1f))
              .AppendCallback(() => starScoreCanvasObject.SetActive(false))
              .AppendCallback(() => starCounter.DOSizeDelta(new Vector2(starCounterTargetWidth, starCounter.sizeDelta.y), 0.2f))
              .AppendCallback(() => starCounterImage.sprite = newStarCounterSprite)
              .AppendCallback(() => starCounterImage.pixelsPerUnitMultiplier = 5.5f)
              .AppendCallback(() => starCircle.sprite = newStarCircleSprite)
              .AppendCallback(() => tickObject.gameObject.SetActive(true))
              .Append(tickObject.DOShakeScale(0.5f, 0.25f))
              .AppendCallback(() => levelCanvasControl._levelTopPartContainer.Hide())
              .AppendCallback(() => levelCanvasControl._coinContainer.Show());
    }

    public void ClaimCoin() {
        Sequence coinSeq = DOTween.Sequence();

        coinSeq.AppendCallback(() => levelCanvasControl._newTileUnlockContainer.Show());
    }
}
