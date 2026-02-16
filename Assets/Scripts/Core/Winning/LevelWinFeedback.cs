using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections;

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

    [SerializeField] private ParticleSystem coinShineParticles;

    [SerializeField] private GameObject fireworkParticleObject;


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
              .AppendCallback(() => AudioManager.Instance.PlaySound("Other", "Level Complete"))
              .Append(tickObject.DOShakeScale(0.5f, 0.25f))
              .AppendCallback(() => levelCanvasControl._coinContainer.Show())
              .AppendCallback(() => coinShineParticles.Play())
              .AppendCallback(() => LevelControl.Instance.CameraPresentation())
              .AppendCallback(() => CreatingFireworks())
              .AppendCallback(() => LevelCompletedTextAnimation());

    }

    public void EndingWonLevel()
    {
        Sequence endSeq = DOTween.Sequence();

        endSeq.AppendCallback(levelCanvasControl.DisableWinContainers)
              .AppendCallback(() => AudioManager.Instance.PlaySound("Other", "Level Complete"))
              .Append(tickObject.DOShakeScale(0.5f, 0.25f))
              .AppendCallback(() => levelCanvasControl._levelTopPartContainer.Hide())
              .AppendCallback(() => levelCanvasControl.ContinueToNextLevelPanel());

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


    public void CreatingFireworks()
    {
        StartCoroutine(fireworkCoroutine());
    }

    private IEnumerator fireworkCoroutine()
    {
        Vector3 position1 = LevelControl.Instance._tileControl.Tilegrid.TileGridCenter();

        Vector3 randomOffset = new Vector3(
        Random.Range(-5f, 5f),
        0f,
        Random.Range(-5f, 5f));

        Vector3 position2 = position1 + randomOffset;
        Vector3 position3 = position1 - randomOffset;

        List<Vector3> newSet = new List<Vector3>();
        newSet.Add(position1);
        newSet.Add(position2);
        newSet.Add(position3);

        newSet = newSet.OrderBy(x => Random.value).ToList();

        Instantiate(fireworkParticleObject, newSet[0], Quaternion.identity);
        yield return new WaitForSeconds(3f);
        Instantiate(fireworkParticleObject, newSet[1], Quaternion.identity);
        yield return new WaitForSeconds(3f);
        Instantiate(fireworkParticleObject, newSet[2], Quaternion.identity);
        yield return new WaitForSeconds(3f);
        StartCoroutine(fireworkCoroutine());
    }
}
