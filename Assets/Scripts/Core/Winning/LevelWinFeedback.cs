using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LevelWinFeedback : MonoBehaviour
{
    [SerializeField] private LevelCanvasControl levelCanvasControl;
    [SerializeField] private CoinCollecting coinCollecting;

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI rewardCoinText;

    [SerializeField] private Image starCircle;
    [SerializeField] private RectTransform starCounter;
    [SerializeField] private Image starCounterImage;
    [SerializeField] private GameObject starScoreCanvasObject;
    [SerializeField] private CanvasGroup starScoreCanvasGroup;
    [SerializeField] private RectTransform tickObject;

    [SerializeField] private RectTransform levelCompletedTextTransform;

    [SerializeField] private Sprite newStarCircleSprite;
    [SerializeField] private Sprite newStarCounterSprite;


    private float starCounterTargetWidth = 85f;

    public TextMeshProUGUI _coinText => coinText;

    public RectTransform _levelCompletedTextTransform => levelCompletedTextTransform;

    public void LevelWinning()
    {
        int currentLevel = GameData.Instance.currentLevelID;
        LevelDataSO currentlevelDataSO = SaveLoadManager.Instance.levelDatas[currentLevel - 1];

        rewardCoinText.text = currentlevelDataSO.coinValue.ToString();

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
              .AppendCallback(() => levelCanvasControl._coinContainer.Show())
              .AppendCallback(() => LevelCompletedTextAnimation());


    }

    public void LevelCompletedTextAnimation()
    {
        Sequence winSeq = DOTween.Sequence();

        winSeq.Append(levelCompletedTextTransform.DOScale(0, 0))
              .AppendCallback(() => levelCompletedTextTransform.gameObject.SetActive(true))
              .Append(levelCompletedTextTransform.DOScale(1f, 0.3f))
              .Append(levelCompletedTextTransform.DOScale(0.7f, 0.1f)).SetEase(Ease.InSine)
              .Append(levelCompletedTextTransform.DOScale(1f, 0.1f)).SetEase(Ease.OutSine);
    }

}
