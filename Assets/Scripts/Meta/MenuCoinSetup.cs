using UnityEngine;
using TMPro;
using DG.Tweening;

public class MenuCoinSetup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DOVirtual.DelayedCall(0.02f, () =>
        {
            coinText.text = SaveLoadManager.Instance.gameData.progressData.coin.ToString();
        });
    }

}
