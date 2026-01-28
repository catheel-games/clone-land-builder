using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class StarCollecting : MonoBehaviour
{
    [SerializeField] private Transform starCircleTransform;
    [SerializeField] private RectTransform starScoreTransform;
    [SerializeField] private TextMeshProUGUI starText1;
    [SerializeField] private TextMeshProUGUI starText2;
    [SerializeField] private TextMeshProUGUI tileText;

    private int starAmount = 0;
    private int tileAmount = 0;
    private List<GameObject> starObjects = new List<GameObject>();
    private GameObject tileObject;

    public void SetMaximumStars() {
        //Set starText2 to LevelData Max
    }

    public void GetPlusObjects(int starAmountRef, List<GameObject> starObjectsRef, GameObject tileObjectRef)
    {
        starAmount = starAmountRef;
        starObjects = starObjectsRef;
        tileObject = tileObjectRef;
    }

    public void SetAcceptFeedback()
    {
        LevelControl.Instance.tileScore--;
        tileText.text = LevelControl.Instance.tileScore.ToString();

        if (tileObject != null) {
            switch (starAmount) {
                case 3:
                    tileAmount = 1; break;
                case 4:
                    tileAmount = 2; break;
                case 5:
                    tileAmount = 3; break;
                case 6:
                    tileAmount = 6; break;
            }
        }
        else if (tileObject == null)
            tileAmount = 0;


        Sequence feedbackSeq = DOTween.Sequence();

        feedbackSeq.Join(starScoreTransform.DOAnchorPosX(0, 0.1f))
                   .Append(starCircleTransform.DOScale(1.25f, 0.2f))

                   .AppendCallback(() => SetScore(LevelControl.Instance.tileScore, tileAmount, tileText))
                   .AppendCallback(() => SetScore(LevelControl.Instance.starScore, starAmount, starText1))
                   .AppendCallback(() => LevelControl.Instance.tileScore += tileAmount)
                   .AppendCallback(() => LevelControl.Instance.starScore += starAmount)

                   .Append(starCircleTransform.DOScale(1f, 0.2f));
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
}
