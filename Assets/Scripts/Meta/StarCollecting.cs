using UnityEngine;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class StarCollecting : Singleton<StarCollecting>
{
    [SerializeField] private LevelWinFeedback levelWinFeedback;

    [SerializeField] private Transform starCircleTransform;
    [SerializeField] private Transform tileCounterTransform;
    [SerializeField] private RectTransform starScoreTransform;
    [SerializeField] private TextMeshProUGUI starText1;
    [SerializeField] private TextMeshProUGUI starText2;
    [SerializeField] private TextMeshProUGUI tileText;

    [Header("Instantiating Prefabs")]
    [SerializeField] private RectTransform tileFlyPrefab;
    [SerializeField] private RectTransform starFlyPrefab;
    [SerializeField] private Transform canvasTransform;

    [Header("Targets")]
    [SerializeField] private RectTransform starTarget;
    [SerializeField] private RectTransform tileTarget;

    [Header("Star Plus Sounds")]
    [SerializeField] private string[] starPlusesSounds;
    private string currentPlusSound;

    [Header("Star Pre-Accept Scoring")]
    [SerializeField] private float offMaskPosition = 125f;
    [SerializeField] private float slideDuration = 0.2f;
    [SerializeField] private RectTransform starScoreMainText;
    [SerializeField] private RectTransform starScoreBonusText;
    [SerializeField] private TextMeshProUGUI starScoringBonus;

    private int starAmount = 0;
    private int tileAmount = 0;
    private List<GameObject> starObjects = new();
    private GameObject tileObject;

    private List<RectTransform> spawnedStars = new();
    private List<RectTransform> spawnedTiles = new();

    private Sequence starScoringBonusSlide;
    private bool isStarScoreBonusMode = false;
    private int starScoreBonusAmount = 0;
    private int maxStarScore;

    void Start()
    {
        SetStarScoreMainMode();
    }

    public void SetDataUI(int maxStarValue)
    {
        starText1.text = LevelControl.Instance.starScore.ToString();
        starText2.text = maxStarValue.ToString();
        tileText.text = LevelControl.Instance.tileScore.ToString();

        maxStarScore = maxStarValue;
        levelWinFeedback._coinText.text = GameData.Instance.coin.ToString();
    }

    public void GetPlusObjects(int starAmountRef, List<GameObject> starObjectsRef, GameObject tileObjectRef)
    {
        starAmount = starAmountRef;
        starObjects = starObjectsRef;
        tileObject = tileObjectRef;
    }

    public void SetAcceptFeedback()
    {
        currentPlusSound = starPlusesSounds[Random.Range(0, starPlusesSounds.Length)];

        spawnedStars.Clear();
        spawnedTiles.Clear();

        LevelControl.Instance.tileScore--;
        tileText.text = LevelControl.Instance.tileScore.ToString();
        tileText.transform.DOScale(1.2f, 0.1f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo);

        if (starAmount != 0)
        {

            if (tileObject != null)
            {
                switch (starAmount)
                {
                    case 3:
                        tileAmount = 1; break;
                    case 4:
                        tileAmount = 2; break;
                    case 5:
                        tileAmount = 3; break;
                    case 6:
                        tileAmount = 6; break;
                }

                for (int i = 0; i < tileAmount; i++)
                {
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(tileObject.transform.position);
                    SpawnTileFly(screenPos);
                }
                Destroy(tileObject);
            }

            else if (tileObject == null)
                tileAmount = 0;

            for (int i = 0; i < starObjects.Count; i++)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(starObjects[i].transform.position);
                SpawnStarFly(screenPos);
                Destroy(starObjects[i]);
            }

            //-------------------------------------------------------- SEQUENCES ------------------------------------------------------------------------------------------

            Sequence starsSeq = DOTween.Sequence();

            Sequence innerStarsSeq = DOTween.Sequence();
            for (int i = 0; i < spawnedStars.Count; i++)
            {
                int index = i;
                innerStarsSeq.AppendCallback(() =>
                {
                    AnimateToSlot(spawnedStars[index], starTarget, false);
                });

                innerStarsSeq.AppendInterval(0.07f);
            }

            starsSeq.Append(innerStarsSeq)
                    .AppendCallback(() => AudioManager.Instance.PlaySound("Plus", currentPlusSound))
                    .AppendInterval(0.125f)
                    .Join(starCircleTransform.DOScale(1.25f, 0.1f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo))
                    .AppendCallback(() => SetScore(LevelControl.Instance.starScore, starAmount, starText1))
                    .AppendCallback(() => LevelControl.Instance.starScore += starAmount)
                    .AppendCallback(CheckWinning);


            starsSeq.Pause();

            Sequence tilesSeq = DOTween.Sequence();

            Sequence innerTilesSeq = DOTween.Sequence();
            for (int i = 0; i < spawnedTiles.Count; i++)
            {
                int index = i;
                innerTilesSeq.AppendCallback(() =>
                {
                    AnimateToSlot(spawnedTiles[index], tileTarget, true);
                });

                innerTilesSeq.AppendInterval(0.05f);

            }

            tilesSeq.Append(innerTilesSeq)
                    //.AppendCallback(() => AudioManager.Instance.PlaySound("Plus", "Tile Plus"))
                    .AppendInterval(0.1f)
                    .Join(tileCounterTransform.DOScale(1.25f, 0.1f).SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo))
                    .AppendCallback(() => SetScore(LevelControl.Instance.tileScore, tileAmount, tileText))
                    .AppendCallback(() => LevelControl.Instance.tileScore += tileAmount);

            tilesSeq.Pause();

            Sequence wholeSeq = DOTween.Sequence();

            wholeSeq.Insert(0f, starScoreTransform.DOAnchorPosX(0, 0.1f))
                    .Insert(0.1f, starsSeq)
                    .InsertCallback(1f, LevelControl.Instance.SaveTile);

            if (tileAmount != 0) {
                wholeSeq.Insert(0.45f, tilesSeq);
            }


        }
    }

    public void SetStarScoreMainMode()
    {   
        if (isStarScoreBonusMode)
        {
            isStarScoreBonusMode = false;

            if (starScoringBonusSlide == null || !starScoringBonusSlide.IsActive())
            {
                Sequence slideSequence = DOTween.Sequence();

                slideSequence.Append(starScoreMainText.DOAnchorPosX(0, slideDuration));
                slideSequence.Join(starScoreBonusText.DOAnchorPosX(-offMaskPosition, slideDuration));
                slideSequence.AppendCallback(() =>
                {
                    starScoreMainText.anchoredPosition = new Vector2(0, starScoreMainText.anchoredPosition.y);
                    starScoreBonusText.anchoredPosition = new Vector2(offMaskPosition, starScoreBonusText.anchoredPosition.y);

                    if (isStarScoreBonusMode)
                    {
                        SetStarScoreBonusMode(starScoreBonusAmount);
                    }
                });

                starScoringBonusSlide = slideSequence;
            }
        }
    }

    public void SetStarScoreBonusMode(int starAmount)
    {
        starScoreBonusAmount = starAmount;
        starScoringBonus.text = "+" + starAmount;

        if (!isStarScoreBonusMode)
        {
            isStarScoreBonusMode = true;

            if (starScoringBonusSlide == null || !starScoringBonusSlide.IsActive())
            {
                Sequence slideSequence = DOTween.Sequence();

                slideSequence.Append(starScoreMainText.DOAnchorPosX(-offMaskPosition, slideDuration));
                slideSequence.Join(starScoreBonusText.DOAnchorPosX(0, slideDuration));
                slideSequence.AppendCallback(() =>
                {
                    starScoreMainText.anchoredPosition = new Vector2(offMaskPosition, starScoreMainText.anchoredPosition.y);
                    starScoreBonusText.anchoredPosition = new Vector2(0, starScoreBonusText.anchoredPosition.y);

                    if (!isStarScoreBonusMode)
                    {
                        SetStarScoreMainMode();
                    }
                });

                starScoringBonusSlide = slideSequence;
            }
        }
    }

    private void SetScore(int lastAmount, int plusAmount, TextMeshProUGUI text)
    {
        int finalAmount = lastAmount + plusAmount;

        if (plusAmount <= 1) {
            text.text = finalAmount.ToString();
        }

        else {
            Sequence seq = DOTween.Sequence();

            for (int i = 0; i < plusAmount; i++)
            {
                int value = lastAmount + i + 1;

                seq.AppendCallback(() => {
                    text.text = value.ToString();
                });
                seq.AppendInterval(0.05f);
            }
        }
    }

    private void SpawnStarFly(Vector3 screenPos)
    {
        RectTransform fly = Instantiate(starFlyPrefab, canvasTransform);
        fly.position = screenPos;

        spawnedStars.Add(fly);

        fly.localScale = Vector3.one * 0.8f;
        fly.DOMoveY(100f, 0.075f).SetRelative();
    }

    private void SpawnTileFly(Vector3 screenPos)
    {
        RectTransform fly = Instantiate(tileFlyPrefab, canvasTransform);

        Vector3 randomOffset = new Vector3(
            Random.Range(-50f, 50f),
            Random.Range(-50f, 50f),
            0f                          );

        fly.position = screenPos + randomOffset;

        spawnedTiles.Add(fly);

        fly.localScale = Vector3.one * 0.8f;
    }

    private void AnimateToSlot(RectTransform fly, RectTransform target, bool withVoice)
    {
        fly.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        fly.DOMove(target.position, 0.35f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                if (withVoice)
                    AudioManager.Instance.PlaySound("Plus", "Tile Plus");

                VibrationManager.Instance.Vibrate();
                Destroy(fly.gameObject);
            });
    }


    private void CheckWinning()
    {
        if (LevelControl.Instance.starScore >= maxStarScore)
        {
            LevelControl.Instance.Winning();
        }
    }
}
