using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class LoadingScreenContainer : FadingContainer
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;

    [SerializeField] private Transform titleTransform;
    [SerializeField] private Transform progressBarTransform;

    [SerializeField] private GameObject[] dots;
    
    public void SetProgress(float progress)
    {
        progressBar.value = progress;
        progressText.text = progress + "%";
    }

    public void TitleScaling()
    {
        titleTransform.DOScale(0, 0f);
        progressBarTransform.DOScale(0, 0f);

        for (int i = 0; i < dots.Length; i++) {
            dots[i].SetActive(false);
        }

        Sequence setupSeq = DOTween.Sequence();

        setupSeq.Append(titleTransform.DOScale(1, 0.25f))
                .AppendInterval(0.05f)
                .Append(progressBarTransform.DOScale(1, 0.1f))
                .AppendCallback(() => StartCoroutine(StartDotting()));

    }

    public IEnumerator StartDotting()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(true);
            yield return new WaitForSeconds(0.25f);
        }
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(false);
            yield return new WaitForSeconds(0.25f);
        }

        StartCoroutine(StartDotting());
    }

}
