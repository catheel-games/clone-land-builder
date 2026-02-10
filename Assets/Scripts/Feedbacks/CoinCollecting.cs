using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

public class CoinCollecting : MonoBehaviour
{
    [SerializeField] private LevelCanvasControl levelCanvasControl;
    [SerializeField] private LevelWinFeedback levelWinFeedback;

    private List<RectTransform> spawnedCoins = new();
    [SerializeField] private RectTransform coinFlyPrefab;
    [SerializeField] private RectTransform targetCoinRectTransform;
    [SerializeField] private RectTransform canvasTransform;
    [SerializeField] private RectTransform coinPanelTransform;

    [SerializeField] private int coinCount;
    [SerializeField] private float animationDelay = 0.04f;

    [SerializeField] private int coinSeed = 298;


    private int coinValue;
    private bool coinSetCalled = false;

    public void ClaimCoin()
    {
        int currentLevel = GameData.Instance.currentLevelID;
        LevelDataSO currentlevelDataSO = SaveLoadManager.Instance.levelDatas[currentLevel - 1];

        coinValue = currentlevelDataSO.coinValue;

        Sequence coinSeq = DOTween.Sequence();

        coinSeq.AppendCallback(() => levelCanvasControl._newTileUnlockContainer.Show())
               .AppendCallback(() => SpawningCoins());
    }

    private void SpawningCoins()
    {
        UnityEngine.Random.InitState(coinSeed);

        float delay = 0f;

        foreach (RectTransform flyRect in spawnedCoins)
        {
            AnimateToSlot(flyRect, delay);
            delay += 0.06f;
        }

        Sequence coinSeq = DOTween.Sequence();

        coinSeq.AppendCallback(() =>
        {
            for (int i = 0; i < coinCount; i++)
                SpawnCoinFly();
        })
                .AppendInterval(0.2f)
                .AppendCallback(() =>
                {
                    foreach (RectTransform flyRect in spawnedCoins)
                    {
                        AnimateToSlot(flyRect, delay);
                        delay += 0.04f;
                    }
                })
                .AppendCallback(() => spawnedCoins.Clear())
                .AppendInterval(1.5f)
                .AppendCallback(() => coinSetCalled = false);

    }

    private void SpawnCoinFly()
    {
        RectTransform fly = Instantiate(coinFlyPrefab, canvasTransform);

        Vector2 randomOffset = new Vector2(
            UnityEngine.Random.Range(-50f, 50f),
            UnityEngine.Random.Range(-50f, 50f));

        Vector2 center = canvasTransform.rect.center;

        fly.anchoredPosition = center + randomOffset;

        spawnedCoins.Add(fly);

        float moveDistance = 80f;
        Vector2 direction = randomOffset.normalized;
        Vector2 targetPos = randomOffset + direction * moveDistance;

        fly.DOAnchorPos(targetPos, 0.4f)
           .SetEase(Ease.OutQuad);
    }

    private void AnimateToSlot(RectTransform fly, float delay)
    {
        Vector2 targetPos = canvasTransform.InverseTransformPoint(
                targetCoinRectTransform.position
            );

        fly.DOAnchorPos(targetPos, 0.5f)
            .SetEase(Ease.OutCubic)
            .SetDelay(delay)
            .OnComplete(() =>
            {
                // AudioManager.Instance.PlaySound("Other", "Coin");
                if (!coinSetCalled)
                {
                    coinSetCalled = true;
                    SetCoin(GameData.Instance.coin, coinValue, levelWinFeedback._coinText);
                    ScalingCoinPanel(1.1f, 0.125f);
                }

                VibrationManager.Instance.Vibrate();
                Destroy(fly.gameObject);
            });


    }

    private void SetCoin(int lastAmount, int plusAmount, TextMeshProUGUI text)
    {
        int finalAmount = lastAmount + plusAmount;
        float duration = coinCount * animationDelay;

        int currentValue = lastAmount;

        DOTween.To(() => currentValue,
            x => {
                currentValue = x;
                text.text = currentValue.ToString();
            },
            finalAmount, duration).SetEase(Ease.Linear).OnComplete(()=>
                ScalingCoinPanel(1.2f, 0.2f)
            );

        GameData.Instance.coin = currentValue;
    }

    private void ScalingCoinPanel(float scaleValue, float duration)
    {
        coinPanelTransform.DOScale(Vector3.one * scaleValue, duration)
        .SetEase(Ease.OutQuad)
        .OnComplete(() =>
        {
            coinPanelTransform
                .DOScale(Vector3.one, duration)
                .SetEase(Ease.InQuad);
        });
    }
}
